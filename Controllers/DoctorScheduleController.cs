using PatientApi.Models.Entities;
using PatientApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
    }
}
