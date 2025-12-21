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

        [HttpGet("{appointmentId}")]
        public async Task<IActionResult> GetByAppointmentId(int appointmentId)
        {
            var consultation = await _service.GetByAppointmentIdAsync(appointmentId);
            if (consultation == null) return NotFound("Consultation not found for this appointment");
            return Ok(consultation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ConsultationRequest request)
        {
            var result = await _service.UpdateAsync(id, request);
            if (!result) return NotFound("Consultation not found");
            return Ok("Consultation updated successfully");
        }
    }
}
