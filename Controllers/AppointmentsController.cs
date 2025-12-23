using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientApi.Data;
using PatientApi.Models.ViewModels;
using PatientApi.Services.Interfaces;
using PatientApi.Extensions;

namespace PatientApi.Controllers
{
    [ApiController]
    [Route("api/appointments")]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _service;
        private readonly AppDbContext _db;

        public AppointmentsController(IAppointmentService service, AppDbContext db)
        {
            _service = service;
            _db = db;
        }

        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Create([FromBody] AppointmentRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!User.TryGetUserId(out var userId)) return Unauthorized();

            var patientId = await _db.Patients
                .Where(p => p.UserId == userId)
                .Select(p => (int?)p.Id)
                .FirstOrDefaultAsync();

            if (!patientId.HasValue)
            {
                return Unauthorized("Patient profile not found.");
            }

            try
            {
                var id = await _service.CreateAsync(patientId.Value, request);
                return Ok(new { AppointmentId = id });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("status")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> UpdateStatus([FromBody] AppointmentStatusRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            int? actingDoctorId = null;
            if (User.IsInRole("Doctor"))
            {
                if (!User.TryGetUserId(out var userId)) return Unauthorized();
                actingDoctorId = await _db.Doctors
                    .Where(d => d.UserId == userId)
                    .Select(d => (int?)d.DoctorId)
                    .FirstOrDefaultAsync();

                if (!actingDoctorId.HasValue)
                {
                    return Unauthorized("Doctor profile not found.");
                }
            }

            var result = await _service.UpdateStatusAsync(request, actingDoctorId);
            return result ? Ok() : NotFound();
        }

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

        [HttpGet]
        [Authorize(Roles = "Admin,Nurse")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }
    }
}
