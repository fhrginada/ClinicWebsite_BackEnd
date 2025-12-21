using Clinical_project.Data;
using Clinical_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientApi.Data;
using PatientApi.Models.ViewModels;
using PatientApi.Models.Entities;

namespace Clinical_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PatientApi.Services.Interfaces.IPatientService _service;

        public PatientsController(AppDbContext context,
                                  PatientApi.Services.Interfaces.IPatientService service)
        {
            _context = context;
            _service = service;
        }

        // =========================
        // Get all patients (simple list)
        // =========================
        [HttpGet("all")]
        public async Task<IActionResult> GetPatients()
        {
            var patients = await _context.Patients.ToListAsync();
            return Ok(patients);
        }

        // =========================
        // Add patient directly to DbContext
        // =========================
        [HttpPost("add")]
        public async Task<IActionResult> AddPatient([FromBody] Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return Ok(patient);
        }

        // =========================
        // Get patients with pagination and optional gender filter
        // =========================
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? gender, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var (total, items) = await _service.GetAsync(gender, page, pageSize);
            return Ok(new { total, page, pageSize, items });
        }

        // =========================
        // Get patient by id
        // =========================
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var p = await _service.GetByIdAsync(id);
            if (p == null) return NotFound();
            return Ok(p);
        }

        // =========================
        // Create patient via ViewModel and service
        // =========================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PatientCreateViewModel vm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var entity = InputMapper.ToEntity(vm);
                var created = await _service.CreateAsync(entity);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // =========================
        // Update patient via ViewModel and service
        // =========================
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] PatientUpdateViewModel vm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                
                var existingPatient = await _service.GetByIdAsync(id);
                if (existingPatient == null) return NotFound();

                
                InputMapper.ApplyUpdate(existingPatient, vm);

                var ok = await _service.UpdateAsync(id, existingPatient);
                return ok ? NoContent() : NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // =========================
        // Delete patient via service
        // =========================
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }

        // =========================
        // Get dashboard data for patient
        // =========================
        [HttpGet("dashboard")]
        [HttpGet("dashboard/{patientId:int}")]
        public async Task<IActionResult> GetDashboard(int patientId)
        {
            var dashboard = await _service.GetDashboardAsync(patientId);
            if (dashboard == null) return NotFound();
            return Ok(dashboard);
        }
    }
}
