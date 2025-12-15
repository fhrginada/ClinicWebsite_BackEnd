using Microsoft.AspNetCore.Mvc;
using PatientApi.DTOs;
using PatientApi.Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class PatientAttachmentsController : ControllerBase
{
    private readonly IPatientAttachmentService _service;
    public PatientAttachmentsController(IPatientAttachmentService service) => _service = service;

    [HttpPost("{patientId:int}")]
    public async Task<IActionResult> Upload(int patientId)
    {
        var file = Request.Form.Files.FirstOrDefault();
        if (file == null) return BadRequest("No file uploaded");
        var created = await _service.UploadAsync(patientId, file);
        if (created == null) return NotFound("Patient not found");
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var att = await _service.GetAsync(id);
        if (att == null) return NotFound();
        return Ok(att);
    }

    [HttpGet("by-patient/{patientId:int}")]
    public async Task<IActionResult> List(int patientId)
    {
        var list = await _service.ListAsync(patientId);
        return Ok(list);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.DeleteAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}
