using Clinical_project.Data;
using Clinical_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clinical_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPatients()
        {
            var patients = await _context.Patients.ToListAsync();
            return Ok(patients);
        }

        [HttpPost]
        public async Task<IActionResult> AddPatient([FromBody] Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return Ok(patient);
        }
    }
}
