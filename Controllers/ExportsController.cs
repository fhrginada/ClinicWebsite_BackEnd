using Microsoft.AspNetCore.Mvc;
using PatientApi.Services.Interfaces;

namespace PatientApi.Controllers
{
    [ApiController]
    [Route("api/exports")]
    public class ExportsController : ControllerBase
    {
        private readonly IExportService _service;

        public ExportsController(IExportService service)
        {
            _service = service;
        }

        [HttpGet("patients/excel")]
        public IActionResult ExportPatients()
        {
            var file = _service.ExportPatientsToExcel();
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }

}
