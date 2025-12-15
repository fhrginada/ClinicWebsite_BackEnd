using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PatientApi.Data;
using PatientApi.DTOs;
using PatientApi.Models;
using PatientApi.Services.Interfaces;

namespace PatientApi.Services;

public class PatientAttachmentService : IPatientAttachmentService
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;
    private readonly string _baseDir;

    public PatientAttachmentService(AppDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
        _baseDir = Path.Combine(_env.ContentRootPath, "wwwroot", "uploads", "patients");
        Directory.CreateDirectory(_baseDir);
    }

    public async Task<PatientAttachmentDto?> UploadAsync(int patientId, IFormFile file)
    {
        var patient = await _db.Patients.FindAsync(patientId);
        if (patient == null) return null;
        var patientDir = Path.Combine(_baseDir, patientId.ToString());
        Directory.CreateDirectory(patientDir);
        var fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
        var relPath = Path.Combine("uploads", "patients", patientId.ToString(), fileName).Replace('\\', '/');
        var fullPath = Path.Combine(patientDir, fileName);
        await using (var stream = System.IO.File.Create(fullPath))
        {
            await file.CopyToAsync(stream);
        }

        var att = new PatientAttachment
        {
            PatientId = patientId,
            FileName = file.FileName,
            ContentType = file.ContentType ?? "application/octet-stream",
            Size = file.Length,
            Path = relPath
        };
        _db.PatientAttachments.Add(att);
        await _db.SaveChangesAsync();

        return new PatientAttachmentDto
        {
            Id = att.Id,
            PatientId = att.PatientId,
            FileName = att.FileName,
            ContentType = att.ContentType,
            Size = att.Size,
            Path = att.Path,
            UploadedAt = att.UploadedAt
        };
    }

    public async Task<PatientAttachmentDto?> GetAsync(int id)
    {
        var a = await _db.PatientAttachments.FindAsync(id);
        if (a == null) return null;
        return new PatientAttachmentDto
        {
            Id = a.Id,
            PatientId = a.PatientId,
            FileName = a.FileName,
            ContentType = a.ContentType,
            Size = a.Size,
            Path = a.Path,
            UploadedAt = a.UploadedAt
        };
    }

    public async Task<IEnumerable<PatientAttachmentDto>> ListAsync(int patientId)
    {
        return await _db.PatientAttachments.Where(a => a.PatientId == patientId).Select(a => new PatientAttachmentDto
        {
            Id = a.Id,
            PatientId = a.PatientId,
            FileName = a.FileName,
            ContentType = a.ContentType,
            Size = a.Size,
            Path = a.Path,
            UploadedAt = a.UploadedAt
        }).ToListAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var a = await _db.PatientAttachments.FindAsync(id);
        if (a == null) return false;
        var fullPath = Path.Combine(_env.ContentRootPath, "wwwroot", a.Path.Replace('/', Path.DirectorySeparatorChar));
        if (System.IO.File.Exists(fullPath)) System.IO.File.Delete(fullPath);
        _db.PatientAttachments.Remove(a);
        await _db.SaveChangesAsync();
        return true;
    }
}
