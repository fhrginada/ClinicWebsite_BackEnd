using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientApi.Data;
using PatientApi.Models.ViewModels;
using PatientApi.Models.Entities;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly PatientApi.Services.Interfaces.IPatientService _service;

    public PatientsController(PatientApi.Services.Interfaces.IPatientService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string? name, [FromQuery] int? gender, [FromQuery] int? ageMin, [FromQuery] int? ageMax, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (total, items) = await _service.GetAsync(name, gender, ageMin, ageMax, page, pageSize);
        return Ok(new { total, page, pageSize, items });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var p = await _service.GetByIdAsync(id);
        if (p == null) return NotFound();
        return Ok(p);
    }

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

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] PatientUpdateViewModel vm)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            // Create a temporary entity and apply updates, service will persist
            var temp = new Patient();
            InputMapper.ApplyUpdate(vm, temp);
            var ok = await _service.UpdateAsync(id, temp);
            return ok ? NoContent() : NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.DeleteAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}
