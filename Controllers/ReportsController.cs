using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PatientApi.Services.Interfaces;

namespace PatientApi.Controllers
{
    [ApiController]
    [Route("api/reports")]
    [Authorize(Roles = "Admin")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _service;

        public ReportsController(IReportService service)
        {
            _service = service;
        }

        [HttpGet("daily")]
        public async Task<IActionResult> Daily([FromQuery] DateTime date)
        {
            return Ok(await _service.GetDailyStatsAsync(date));
        }

        [HttpGet("monthly")]
        public async Task<IActionResult> Monthly(int year, int month)
        {
            return Ok(await _service.GetMonthlyStatsAsync(year, month));
        }
    }

}
