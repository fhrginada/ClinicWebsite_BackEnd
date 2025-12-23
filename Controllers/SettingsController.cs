using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Clinical_project.Services.Settings;
using Clinical_project.Models.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Clinical_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    [Authorize(Roles = "Admin")]
    public class SettingsController : ControllerBase
    {
        private readonly SettingsService _settingsService;

        public SettingsController(SettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        
        [HttpGet]
        [AllowAnonymous] 
        public async Task<IActionResult> GetSettings()
        {
            var settings = await _settingsService.GetSettingsAsync();
            return Ok(settings);
        }

        
        [HttpPut]
        public async Task<IActionResult> UpdateSettings([FromBody] UpdateSettingsRequest request)
        {
            if (request == null)
                return BadRequest("Invalid settings request.");

            var updatedSettings = await _settingsService.UpdateSettingsAsync(request);
            return Ok(updatedSettings);
        }
    }
}