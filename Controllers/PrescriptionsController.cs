using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PatientApi.Models.Entities;
using PatientApi.Models.ViewModels;
using PatientApi.Services;
using PatientApi.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using PatientApi.Data;
using PatientApi.Extensions;

namespace PatientApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPrescriptionService _service;
        private readonly PdfGeneratorService _pdfService;
        private readonly AppDbContext _db;

        public PrescriptionsController(IPrescriptionService service, PdfGeneratorService pdfService, AppDbContext db)
        {
            _service = service;
            _pdfService = pdfService;
            _db = db;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("me")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetMine()
        {
            if (!User.TryGetUserId(out var userId)) return Unauthorized();
            var patientId = await _db.Patients
                .Where(p => p.UserId == userId)
                .Select(p => (int?)p.Id)
                .FirstOrDefaultAsync();

            if (!patientId.HasValue) return Unauthorized("Patient profile not found.");
            return Ok(await _service.GetForPatientAsync(patientId.Value));
        }

        [HttpGet("doctor/me")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorMine()
        {
            if (!User.TryGetUserId(out var userId)) return Unauthorized();
            var doctorId = await _db.Doctors
                .Where(d => d.UserId == userId)
                .Select(d => (int?)d.DoctorId)
                .FirstOrDefaultAsync();

            if (!doctorId.HasValue) return Unauthorized("Doctor profile not found.");
            return Ok(await _service.GetForDoctorAsync(doctorId.Value));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var presc = await _service.GetByIdAsync(id);
            if (presc == null) return NotFound();

            if (User.IsInRole("Patient"))
            {
                if (!User.TryGetUserId(out var userId)) return Unauthorized();
                var patientId = await _db.Patients
                    .Where(p => p.UserId == userId)
                    .Select(p => (int?)p.Id)
                    .FirstOrDefaultAsync();
                if (!patientId.HasValue) return Unauthorized("Patient profile not found.");
                if (presc.PatientId != patientId.Value) return Forbid();
            }
            else if (User.IsInRole("Doctor"))
            {
                if (!User.TryGetUserId(out var userId)) return Unauthorized();
                var doctorId = await _db.Doctors
                    .Where(d => d.UserId == userId)
                    .Select(d => (int?)d.DoctorId)
                    .FirstOrDefaultAsync();
                if (!doctorId.HasValue) return Unauthorized("Doctor profile not found.");
                if (presc.DoctorId != doctorId.Value) return Forbid();
            }

            return Ok(presc);
        }

        [HttpPost]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> Create(PrescriptionRequest request)
        {
            if (request.DoctorId.HasValue || request.PatientId.HasValue)
            {
                return BadRequest("DoctorId and PatientId must not be provided.");
            }

            var consultation = await _db.Consultations
                .Include(c => c.Appointment)
                .FirstOrDefaultAsync(c => c.Id == request.ConsultationId);

            if (consultation?.Appointment == null) return BadRequest("Invalid ConsultationId.");

            var appointment = consultation.Appointment;
            var doctorId = appointment.DoctorId;
            var patientId = appointment.PatientId;

            if (User.IsInRole("Doctor"))
            {
                if (!User.TryGetUserId(out var userId)) return Unauthorized();
                var myDoctorId = await _db.Doctors
                    .Where(d => d.UserId == userId)
                    .Select(d => (int?)d.DoctorId)
                    .FirstOrDefaultAsync();
                if (!myDoctorId.HasValue) return Unauthorized("Doctor profile not found.");
                if (myDoctorId.Value != doctorId) return Forbid();
            }

            var prescription = new Prescription
            {
                ConsultationId = request.ConsultationId,
                DoctorId = doctorId,
                PatientId = patientId,
                Status = string.IsNullOrWhiteSpace(request.Status) ? "draft" : request.Status,
                CreatedAt = DateTime.UtcNow,
                Items = request.Items.Select(i => new PrescriptionDetail
                {
                    MedicationId = i.MedicationId,
                    Dose = i.Dose,
                    Frequency = i.Frequency
                }).ToList()
            };

            await _service.AddAsync(prescription);
            return CreatedAtAction(nameof(GetById), new { id = prescription.PrescriptionId }, prescription);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> Update(int id, PrescriptionRequest request)
        {
            var presc = await _service.GetByIdAsync(id);
            if (presc == null) return NotFound();

            if (request.DoctorId.HasValue || request.PatientId.HasValue)
            {
                return BadRequest("DoctorId and PatientId must not be provided.");
            }

            if (User.IsInRole("Doctor"))
            {
                if (!User.TryGetUserId(out var userId)) return Unauthorized();
                var myDoctorId = await _db.Doctors
                    .Where(d => d.UserId == userId)
                    .Select(d => (int?)d.DoctorId)
                    .FirstOrDefaultAsync();
                if (!myDoctorId.HasValue) return Unauthorized("Doctor profile not found.");
                if (presc.DoctorId != myDoctorId.Value) return Forbid();
            }

            presc.Items = request.Items.Select(i => new PrescriptionDetail
            {
                MedicationId = i.MedicationId,
                Dose = i.Dose,
                Frequency = i.Frequency
            }).ToList();

            await _service.UpdateAsync(presc);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpGet("{id}/pdf")]
        public async Task<IActionResult> ExportPdf(int id)
        {
            var presc = await _service.GetByIdAsync(id);
            if (presc == null) return NotFound();

            if (User.IsInRole("Patient"))
            {
                if (!User.TryGetUserId(out var userId)) return Unauthorized();
                var patientId = await _db.Patients
                    .Where(p => p.UserId == userId)
                    .Select(p => (int?)p.Id)
                    .FirstOrDefaultAsync();
                if (!patientId.HasValue) return Unauthorized("Patient profile not found.");
                if (presc.PatientId != patientId.Value) return Forbid();
            }
            else if (User.IsInRole("Doctor"))
            {
                if (!User.TryGetUserId(out var userId)) return Unauthorized();
                var doctorId = await _db.Doctors
                    .Where(d => d.UserId == userId)
                    .Select(d => (int?)d.DoctorId)
                    .FirstOrDefaultAsync();
                if (!doctorId.HasValue) return Unauthorized("Doctor profile not found.");
                if (presc.DoctorId != doctorId.Value) return Forbid();
            }

            var pdfBytes = await _pdfService.GeneratePrescriptionPdfAsync(presc);
            return File(pdfBytes, "application/pdf", $"Prescription_{id}.pdf");
        }
    }
}
