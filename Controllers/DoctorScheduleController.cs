using PatientApi.Models.Entities;
using PatientApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PatientApi.Data;
using PatientApi.Extensions;

namespace PatientApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DoctorScheduleController : ControllerBase
    {
        private readonly IDoctorScheduleService _service;
        private readonly AppDbContext _db;

        public DoctorScheduleController(IDoctorScheduleService service, AppDbContext db)
        {
            _service = service;
            _db = db;
        }

        [HttpGet("me")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetMine()
        {
            if (!User.TryGetUserId(out var userId)) return Unauthorized();
            var doctorId = await _db.Doctors
                .Where(d => d.UserId == userId)
                .Select(d => (int?)d.DoctorId)
                .FirstOrDefaultAsync();

            if (!doctorId.HasValue) return Unauthorized("Doctor profile not found.");
            return Ok(await _service.GetByDoctorIdAsync(doctorId.Value));
        }

        [HttpGet("doctor/{doctorId}")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> GetByDoctor(int doctorId)
        {
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

            return Ok(await _service.GetByDoctorIdAsync(doctorId));
        }

        [HttpPost]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> Create(DoctorSchedule schedule)
        {
            if (User.IsInRole("Doctor"))
            {
                if (!User.TryGetUserId(out var userId)) return Unauthorized();
                var doctorId = await _db.Doctors
                    .Where(d => d.UserId == userId)
                    .Select(d => (int?)d.DoctorId)
                    .FirstOrDefaultAsync();
                if (!doctorId.HasValue) return Unauthorized("Doctor profile not found.");
                schedule.DoctorId = doctorId.Value;
            }

            await _service.AddAsync(schedule);
            return Ok(schedule);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("availability/{doctorId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvailability(int doctorId, [FromQuery] DateTime? startDate = null, [FromQuery] int days = 7)
        {
            var availability = await _service.GetDoctorAvailabilityAsync(doctorId, startDate, days);
            if (availability == null) return NotFound("Doctor not found");
            return Ok(availability);
        }
    }
}
