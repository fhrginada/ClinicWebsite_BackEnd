using ClinicalProject_API.Models.ViewModels.DoctorVM;
using ClinicalProject_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicalProject_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        // POST: api/Doctor
        [HttpPost]
        public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorRequest request)
        {
            var result = await _doctorService.CreateDoctorAsync(request);
            return Ok(result);
        }

        // GET: api/Doctor
        [HttpGet]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _doctorService.GetAllDoctorsAsync();
            return Ok(doctors);
        }

        // GET: api/Doctor/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null)
                return NotFound("Doctor not found");

            return Ok(doctor);
        }

        // PUT: api/Doctor/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] UpdateDoctorRequest request)
        {
            var updated = await _doctorService.UpdateDoctorAsync(id, request);
            if (!updated)
                return NotFound("Doctor not found");

            return Ok("Doctor updated successfully");
        }

        // DELETE: api/Doctor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var deleted = await _doctorService.DeleteDoctorAsync(id);
            if (!deleted)
                return NotFound("Doctor not found");

            return Ok("Doctor deleted successfully");
        }
    }
}
