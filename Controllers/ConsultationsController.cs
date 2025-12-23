using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientApi.Models.ViewModels;
using PatientApi.Services.Interfaces;
using PatientApi.Data;
using PatientApi.Extensions;

namespace PatientApi.Controllers
{
    [ApiController]
    [Route("api/consultations")]
    [Authorize]
    public class ConsultationsController : ControllerBase
    {
        private readonly IConsultationService _service;
        private readonly AppDbContext _db;

        public ConsultationsController(IConsultationService service, AppDbContext db)
        {
            _service = service;
            _db = db;
        }

        [HttpPost]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> Create(ConsultationRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (User.IsInRole("Doctor"))
            {
                if (!User.TryGetUserId(out var userId)) return Unauthorized();
                var doctorId = await _db.Doctors
                    .Where(d => d.UserId == userId)
                    .Select(d => (int?)d.DoctorId)
                    .FirstOrDefaultAsync();

                if (!doctorId.HasValue) return Unauthorized("Doctor profile not found.");
                var apptDoctorId = await _db.Appointments
                    .Where(a => a.AppointmentId == request.AppointmentId)
                    .Select(a => (int?)a.DoctorId)
                    .FirstOrDefaultAsync();

                if (!apptDoctorId.HasValue) return BadRequest("Invalid AppointmentId.");
                if (apptDoctorId.Value != doctorId.Value) return Forbid();
            }

            await _service.CreateAsync(request);
            return Ok();
        }

        [HttpGet("{appointmentId}")]
        public async Task<IActionResult> GetByAppointmentId(int appointmentId)
        {
            if (User.IsInRole("Patient"))
            {
                if (!User.TryGetUserId(out var userId)) return Unauthorized();
                var patientId = await _db.Patients
                    .Where(p => p.UserId == userId)
                    .Select(p => (int?)p.Id)
                    .FirstOrDefaultAsync();
                if (!patientId.HasValue) return Unauthorized("Patient profile not found.");

                var apptPatientId = await _db.Appointments
                    .Where(a => a.AppointmentId == appointmentId)
                    .Select(a => (int?)a.PatientId)
                    .FirstOrDefaultAsync();

                if (!apptPatientId.HasValue) return NotFound();
                if (apptPatientId.Value != patientId.Value) return Forbid();
            }
            else if (User.IsInRole("Doctor"))
            {
                if (!User.TryGetUserId(out var userId)) return Unauthorized();
                var doctorId = await _db.Doctors
                    .Where(d => d.UserId == userId)
                    .Select(d => (int?)d.DoctorId)
                    .FirstOrDefaultAsync();
                if (!doctorId.HasValue) return Unauthorized("Doctor profile not found.");

                var apptDoctorId = await _db.Appointments
                    .Where(a => a.AppointmentId == appointmentId)
                    .Select(a => (int?)a.DoctorId)
                    .FirstOrDefaultAsync();

                if (!apptDoctorId.HasValue) return NotFound();
                if (apptDoctorId.Value != doctorId.Value) return Forbid();
            }

            var consultation = await _service.GetByAppointmentIdAsync(appointmentId);
            if (consultation == null) return NotFound("Consultation not found for this appointment");
            return Ok(consultation);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> Update(int id, ConsultationRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _service.UpdateAsync(id, request);
            if (!result) return NotFound("Consultation not found");
            return Ok("Consultation updated successfully");
        }
    }
}
