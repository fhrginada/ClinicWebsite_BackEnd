using Microsoft.AspNetCore.Mvc;
using PatientApi.Models.ViewModels;
using PatientApi.Services.Interfaces;

namespace PatientApi.Controllers
{
    [ApiController]
    [Route("api/consultations")]
    public class ConsultationsController : ControllerBase
    {
        private readonly IConsultationService _service;

        public ConsultationsController(IConsultationService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ConsultationRequest request)
        {
            await _service.CreateAsync(request);
            return Ok();
        }
    }
}
