using PatientApi.Models.ViewModels.DoctorVM;
using PatientApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PatientApi.Models.Entities;

namespace PatientApi.Controllers
{
    [ApiController]
    [Route("api/doctors")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly IDoctorScheduleService _scheduleService;

        public DoctorController(IDoctorService doctorService, IDoctorScheduleService scheduleService)
        {
            _doctorService = doctorService;
            _scheduleService = scheduleService;
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

        [HttpGet("{id:int}/availability")]
        public async Task<IActionResult> GetAvailability(int id, [FromQuery] DateTime? startDate = null, [FromQuery] int days = 7)
        {
            var availability = await _scheduleService.GetDoctorAvailabilityAsync(id, startDate, days);
            if (availability == null) return NotFound("Doctor not found");
            return Ok(availability);
        }
    }
}
