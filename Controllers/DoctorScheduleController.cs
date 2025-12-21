using PatientApi.Models.Entities;
using PatientApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PatientApi.Services.Implementations;

namespace PatientApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorScheduleController : ControllerBase
    {
        private readonly IDoctorScheduleService _service;

        public DoctorScheduleController(IDoctorScheduleService service)
        {
            _service = service;
        }

        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetByDoctor(int doctorId)
        {
            return Ok(await _service.GetByDoctorIdAsync(doctorId));
        }

        [HttpPost]
        public async Task<IActionResult> Create(DoctorSchedule schedule)
        {
            await _service.AddAsync(schedule);
            return Ok(schedule);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("availability/{doctorId}")]
        public async Task<IActionResult> GetAvailability(int doctorId, [FromQuery] DateTime? startDate = null, [FromQuery] int days = 7)
        {
            var availability = await _service.GetDoctorAvailabilityAsync(doctorId, startDate, days);
            if (availability == null) return NotFound("Doctor not found");
            return Ok(availability);
        }
    }
}
