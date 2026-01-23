using PatientApi.Models.Entities;
using PatientApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientApi.Data;
using PatientApi.Extensions;

namespace PatientApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NurseScheduleController : ControllerBase
    {
        private readonly INurseScheduleService _service;
        private readonly AppDbContext _db;

        public NurseScheduleController(INurseScheduleService service, AppDbContext db)
        {
            _service = service;
            _db = db;
        }

        [HttpGet("me")]
        [Authorize(Roles = "Nurse")]
        public async Task<IActionResult> GetMine()
        {
            if (!User.TryGetUserId(out var userId)) return Unauthorized();
            var nurseId = await _db.Nurses
                .Where(n => n.UserId == userId)
                .Select(n => (int?)n.NurseId)
                .FirstOrDefaultAsync();

            if (!nurseId.HasValue) return Unauthorized("Nurse profile not found.");
            return Ok(await _service.GetByNurseIdAsync(nurseId.Value));
        }

        [HttpGet("nurse/{nurseId}")]
        [Authorize(Roles = "Nurse,Admin")]
        public async Task<IActionResult> GetByNurse(int nurseId)
        {
            if (!User.TryGetUserId(out var userId)) return Unauthorized();
            if (User.IsInRole("Nurse"))
            {
                var myNurseId = await _db.Nurses
                    .Where(n => n.UserId == userId)
                    .Select(n => (int?)n.NurseId)
                    .FirstOrDefaultAsync();

                if (!myNurseId.HasValue) return Unauthorized("Nurse profile not found.");
                if (myNurseId.Value != nurseId) return Forbid();
            }

            return Ok(await _service.GetByNurseIdAsync(nurseId));
        }

        [HttpPost]
        [Authorize(Roles = "Nurse,Admin")]
        public async Task<IActionResult> Create(NurseSchedule schedule)
        {
            if (!User.TryGetUserId(out var userId)) return Unauthorized();
            if (User.IsInRole("Nurse"))
            {
                var nurseId = await _db.Nurses
                    .Where(n => n.UserId == userId)
                    .Select(n => (int?)n.NurseId)
                    .FirstOrDefaultAsync();

                if (!nurseId.HasValue) return Unauthorized("Nurse profile not found.");
                schedule.NurseId = nurseId.Value;
            }

            await _service.AddAsync(schedule);
            return Ok(schedule);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Nurse,Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
