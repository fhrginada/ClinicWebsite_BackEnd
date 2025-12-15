using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientApi.Data;
using PatientApi.Models;
using PatientApi.DTOs;

[ApiController]
[Route("api/[controller]")]
public class MedicalHistoriesController : ControllerBase
{
    private readonly PatientApi.Services.Interfaces.IMedicalHistoryService _service;
    public MedicalHistoriesController(PatientApi.Services.Interfaces.IMedicalHistoryService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int? patientId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (total, items) = await _service.GetAsync(patientId, page, pageSize);
        return Ok(new { total, page, pageSize, items });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var m = await _service.GetByIdAsync(id);
        if (m == null) return NotFound();
        return Ok(m);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MedicalHistoryCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (dto.Date > DateTime.UtcNow) return BadRequest("Date cannot be in the future");
        var created = await _service.CreateAsync(dto);
        if (created == null) return BadRequest("Patient not found");
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] MedicalHistoryUpdateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (dto.Date > DateTime.UtcNow) return BadRequest("Date cannot be in the future");
        var ok = await _service.UpdateAsync(id, dto);
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.DeleteAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}
