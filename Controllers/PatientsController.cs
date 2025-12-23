using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientApi.Data;
using PatientApi.Models.Entities;
using PatientApi.Models.ViewModels;
using PatientApi.Services.Interfaces;
using PatientApi.Extensions;

namespace PatientApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IPatientService _service;

        public PatientsController(
            AppDbContext context,
            IPatientService service)
        {
            _context = context;
            _service = service;
        }

        // =========================
        // Get all patients (simple list)
        // =========================
        [HttpGet("all")]
        [Authorize(Roles = "Admin,Doctor,Nurse")]
        public async Task<IActionResult> GetPatients()
        {
            var patients = await _context.Patients.ToListAsync();
            return Ok(patients);
        }

        // =========================
        // Add patient directly to DbContext (testing only)
        // =========================
        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddPatient([FromBody] Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return Ok(patient);
        }

        // =========================
        // Get patients with pagination + optional gender filter
        // =========================
        [HttpGet]
        [Authorize(Roles = "Admin,Doctor,Nurse")]
        public async Task<IActionResult> Get(
            [FromQuery] int? gender,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var (total, items) = await _service.GetAsync(gender, page, pageSize);
            return Ok(new
            {
                total,
                page,
                pageSize,
                items
            });
        }

        // =========================
        // Get patient by id (ViewModel)
        // =========================
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (User.IsInRole("Patient"))
            {
                if (!User.TryGetUserId(out var userId)) return Unauthorized();
                var myPatientId = await _context.Patients
                    .Where(p => p.UserId == userId)
                    .Select(p => (int?)p.Id)
                    .FirstOrDefaultAsync();

                if (!myPatientId.HasValue) return Unauthorized("Patient profile not found.");
                if (myPatientId.Value != id) return Forbid();
            }

            var patient = await _service.GetByIdAsync(id);
            if (patient == null)
                return NotFound();

            return Ok(patient);
        }

        // =========================
        // Create patient (ViewModel → Entity)
        // =========================
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] PatientCreateViewModel vm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var entity = InputMapper.ToEntity(vm);
                var created = await _service.CreateAsync(entity);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = created.Id },
                    created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // =========================
        // Update patient (FIXED VERSION ✅)
        // =========================
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] PatientUpdateViewModel vm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // ✅ نجيب Entity مش ViewModel
                var existingPatient = await _service.GetEntityByIdAsync(id);
                if (existingPatient == null)
                    return NotFound();

                // ✅ نعدّل على Entity
                InputMapper.ApplyUpdate(existingPatient, vm);

                var updated = await _service.UpdateAsync(id, existingPatient);
                return updated ? NoContent() : NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // =========================
        // Delete patient
        // =========================
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        // =========================
        // Patient dashboard
        // =========================
        [HttpGet("dashboard/me")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetMyDashboard()
        {
            if (!User.TryGetUserId(out var userId)) return Unauthorized();
            var patientId = await _context.Patients
                .Where(p => p.UserId == userId)
                .Select(p => (int?)p.Id)
                .FirstOrDefaultAsync();

            if (!patientId.HasValue) return Unauthorized("Patient profile not found.");

            var dashboard = await _service.GetDashboardAsync(patientId.Value);
            if (dashboard == null) return NotFound();

            return Ok(dashboard);
        }

        [HttpGet("dashboard/{patientId:int}")]
        [Authorize(Roles = "Admin,Doctor,Nurse")]
        public async Task<IActionResult> GetDashboardForPatient(int patientId)
        {
            var dashboard = await _service.GetDashboardAsync(patientId);
            if (dashboard == null) return NotFound();
            return Ok(dashboard);
        }
    }
}
