using Microsoft.AspNetCore.Mvc;
using PatientApi.Models.Entities;
using PatientApi.Models.ViewModels;
using PatientApi.Services;
using PatientApi.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace PatientApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPrescriptionService _service;
        private readonly PdfGeneratorService _pdfService;

        public PrescriptionsController(IPrescriptionService service, PdfGeneratorService pdfService)
        {
            _service = service;
            _pdfService = pdfService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var presc = await _service.GetByIdAsync(id);
            if (presc == null) return NotFound();
            return Ok(presc);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PrescriptionRequest request)
        {
            var prescription = new Prescription
            {
                ConsultationId = request.ConsultationId,
                Items = request.Items.Select(i => new PrescriptionDetail
                {
                    MedicationId = i.MedicationId,
                    Dose = i.Dose,
                    Frequency = i.Frequency
                }).ToList()
            };

            await _service.AddAsync(prescription);
            return CreatedAtAction(nameof(GetById), new { id = prescription.PrescriptionId }, prescription);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PrescriptionRequest request)
        {
            var presc = await _service.GetByIdAsync(id);
            if (presc == null) return NotFound();

            presc.Items = request.Items.Select(i => new PrescriptionDetail
            {
                MedicationId = i.MedicationId,
                Dose = i.Dose,
                Frequency = i.Frequency
            }).ToList();

            await _service.UpdateAsync(presc);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpGet("{id}/pdf")]
        public async Task<IActionResult> ExportPdf(int id)
        {
            var presc = await _service.GetByIdAsync(id);
            if (presc == null) return NotFound();

            var pdfBytes = await _pdfService.GeneratePrescriptionPdfAsync(presc);
            return File(pdfBytes, "application/pdf", $"Prescription_{id}.pdf");
        }
    }
}
