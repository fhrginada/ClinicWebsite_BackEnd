using Microsoft.AspNetCore.Mvc;
using ClinicBackend_Final.Services.Interfaces;

namespace ClinicBackend_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionExportController : ControllerBase
    {
        private readonly IPdfExportService _service;

        public PrescriptionExportController(IPdfExportService service)
        {
            _service = service;
        }

        [HttpGet("{id}/pdf")]
        public IActionResult Export(int id)
        {
            var fileBytes = _service.ExportPrescriptionToPdf(id);
            return File(fileBytes, "application/pdf", $"Prescription_{id}.pdf");
        }
    }
}
