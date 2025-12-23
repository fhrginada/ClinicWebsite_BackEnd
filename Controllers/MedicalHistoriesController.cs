using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientApi.Data;
using PatientApi.Models.ViewModels;
using PatientApi.Models.Entities;
using PatientApi.Extensions;

[ApiController]
[Route("api/medical-history")]
[Authorize]
public class MedicalHistoriesController : ControllerBase
{
    private readonly PatientApi.Services.Interfaces.IMedicalHistoryService _service;
    private readonly AppDbContext _db;

    public MedicalHistoriesController(PatientApi.Services.Interfaces.IMedicalHistoryService service, AppDbContext db)
    {
        _service = service;
        _db = db;
    }

    [HttpGet]
    [Authorize(Roles = "Doctor,Nurse,Admin")]
    public async Task<IActionResult> Get([FromQuery] int? patientId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (total, items) = await _service.GetAsync(patientId, page, pageSize);
        return Ok(new { total, page, pageSize, items });
    }

    [HttpGet("patient/{patientId:int}")]
    [Authorize(Roles = "Doctor,Nurse,Admin")]
    public async Task<IActionResult> GetByPatient(int patientId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (total, items) = await _service.GetAsync(patientId, page, pageSize);
        return Ok(new { total, page, pageSize, items });
    }

    [HttpGet("me")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> GetMyMedicalHistory([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (!User.TryGetUserId(out var userId)) return Unauthorized();

        var patientId = await _db.Patients
            .Where(p => p.UserId == userId)
            .Select(p => (int?)p.Id)
            .FirstOrDefaultAsync();

        if (!patientId.HasValue) return Unauthorized("Patient profile not found.");

        var (total, items) = await _service.GetAsync(patientId.Value, page, pageSize);
        return Ok(new { total, page, pageSize, items });
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Doctor,Nurse,Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var m = await _service.GetByIdAsync(id);
        if (m == null) return NotFound();
        return Ok(m);
    }

    [HttpPost]
    [Authorize(Roles = "Doctor,Admin")]
    public async Task<IActionResult> Create([FromBody] MedicalHistoryCreateViewModel vm)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (vm.DateRecorded > DateTime.UtcNow) return BadRequest("DateRecorded cannot be in the future");
        var entity = InputMapper.ToEntity(vm);
        var created = await _service.CreateAsync(entity);
        if (created == null) return BadRequest("Patient not found");
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Doctor,Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] MedicalHistoryUpdateViewModel vm)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (vm.DateRecorded > DateTime.UtcNow) return BadRequest("DateRecorded cannot be in the future");
        var temp = new MedicalHistory();
        InputMapper.ApplyUpdate(temp, vm);
        var ok = await _service.UpdateAsync(id, temp);
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Doctor,Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.DeleteAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}
