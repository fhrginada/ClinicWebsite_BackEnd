using Microsoft.AspNetCore.Mvc;
using ClinicBackend_Final.Services.Interfaces;

namespace ClinicBackend_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicationsController : ControllerBase
    {
        private readonly IMedicationService _service;

        public MedicationsController(IMedicationService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());
    }
}
