using ClinicalProject_API.Models.Entities;
using ClinicalProject_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicalProject_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NurseScheduleController : ControllerBase
    {
        private readonly INurseScheduleService _service;

        public NurseScheduleController(INurseScheduleService service)
        {
            _service = service;
        }

        [HttpGet("nurse/{nurseId}")]
        public async Task<IActionResult> GetByNurse(int nurseId)
        {
            return Ok(await _service.GetByNurseIdAsync(nurseId));
        }

        [HttpPost]
        public async Task<IActionResult> Create(NurseSchedule schedule)
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
