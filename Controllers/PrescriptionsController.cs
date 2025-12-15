using ClinicalBackend.Models;
using ClinicalBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicalBackend.Controllers
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var p = await _service.GetByIdAsync(id);
            if (p == null) return NotFound();
            return Ok(p);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Prescription prescription)
        {
            var created = await _service.CreateAsync(prescription);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            // أو return Ok("Prescription created"); لو حابب تبسطها
        }
    }
}