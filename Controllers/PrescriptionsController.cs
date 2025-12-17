using Microsoft.AspNetCore.Mvc;
using ClinicBackend_Final.Services.Interfaces;
using ClinicBackend_Final.ViewModels.PrescriptionViewModels;

namespace ClinicBackend_Final.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPrescriptionService _service;

        public PrescriptionsController(IPrescriptionService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Create(CreatePrescriptionViewModel model)
        {
            var result = _service.Create(model);
            return Ok(result);
        }
    }
}
