using Microsoft.AspNetCore.Mvc;
using PatientApi.Models.ViewModels;
using PatientApi.Services.Interfaces;

namespace PatientApi.Controllers
{
    [ApiController]
    [Route("api/appointments")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _service;

        public AppointmentsController(IAppointmentService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppointmentRequest request)
        {
            var id = await _service.CreateAsync(request);
            return Ok(new { AppointmentId = id });
        }

        [HttpPut("status")]
        public async Task<IActionResult> UpdateStatus(AppointmentStatusRequest request)
        {
            var result = await _service.UpdateStatusAsync(request);
            return result ? Ok() : NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }
    }
}
