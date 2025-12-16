using Clinical_project.Models.Entities;
using Clinical_project.Services.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Clinical_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    [Authorize(Roles = "Admin")]
    public class SettingsController : ControllerBase
    {
        private readonly SettingsService _settingsService;

        public SettingsController(SettingsService settingsService)
        {
            _settingsService = settingsService;
        }

       
        [HttpGet]
        public async Task<IActionResult> GetSettings()
        {
            
            var settings = await _settingsService.GetSettingsAsync();
            return Ok(settings);
        }

       
        [HttpPut]
        public async Task<IActionResult> UpdateSettings([FromBody] UpdateSettingsRequest request) 
        {
            if (request == null)
            {
                return BadRequest("Invalid settings data.");
            }

            var updatedSettings = await _settingsService.UpdateSettingsAsync(request);

            return Ok(updatedSettings);
        }
    }
}
