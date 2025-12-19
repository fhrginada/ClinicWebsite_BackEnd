using PatientApi.Models.ViewModels.NurseVM;
using PatientApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PatientApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NurseController : ControllerBase
    {
        private readonly INurseService _nurseService;

        public NurseController(INurseService nurseService)
        {
            _nurseService = nurseService;
        }

        // POST: api/Nurse
        [HttpPost]
        public async Task<IActionResult> CreateNurse([FromBody] CreateNurseRequest request)
        {
            var result = await _nurseService.CreateAsync(request);
            return Ok(result);
        }

        // GET: api/Nurse
        [HttpGet]
        public async Task<IActionResult> GetAllNurses()
        {
            var nurses = await _nurseService.GetAllAsync();
            return Ok(nurses);
        }

        // GET: api/Nurse/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNurseById(int id)
        {
            var nurse = await _nurseService.GetByIdAsync(id);
            if (nurse == null)
                return NotFound("Nurse not found");

            return Ok(nurse);
        }

        // PUT: api/Nurse/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNurse(int id, [FromBody] UpdateNurseRequest request)
        {
            var updated = await _nurseService.UpdateAsync(id, request);
            if (!updated)
                return NotFound("Nurse not found");

            return Ok("Nurse updated successfully");
        }

        // DELETE: api/Nurse/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNurse(int id)
        {
            var deleted = await _nurseService.DeleteAsync(id);
            if (!deleted)
                return NotFound("Nurse not found");

            return Ok("Nurse deleted successfully");
        }
    }
}
