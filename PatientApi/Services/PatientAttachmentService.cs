using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PatientApi.Data;
using PatientApi.Models;
using PatientApi.Services.Interfaces;

namespace PatientApi.Services;

public class PatientAttachmentService : IPatientAttachmentService
{
    private readonly PatientApi.Services.Repositories.Interfaces.IPatientAttachmentRepository _repo;
    private readonly PatientApi.Services.Repositories.Interfaces.IPatientRepository _patientRepo;
    private readonly IWebHostEnvironment _env;
    private readonly string _baseDir;

    public PatientAttachmentService(PatientApi.Services.Repositories.Interfaces.IPatientAttachmentRepository repo, PatientApi.Services.Repositories.Interfaces.IPatientRepository patientRepo, IWebHostEnvironment env)
    {
        _repo = repo;
        _patientRepo = patientRepo;
        _env = env;
        _baseDir = Path.Combine(_env.ContentRootPath, "wwwroot", "uploads", "patients");
        Directory.CreateDirectory(_baseDir);
    }

    public async Task<PatientAttachment?> UploadAsync(int patientId, IFormFile file)
    {
        var patient = await _patientRepo.FindAsync(patientId);
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
        await _repo.AddAsync(att);
        await _repo.SaveChangesAsync();

        att.Patient = null;
        return att;
    }

    public async Task<PatientAttachment?> GetAsync(int id)
    {
        var a = await _repo.FindAsync(id);
        if (a == null) return null;
        a.Patient = null;
        return a;
    }

    public async Task<IEnumerable<PatientAttachment>> ListAsync(int patientId)
    {
        var list = await _repo.Query().Where(a => a.PatientId == patientId).ToListAsync();
        foreach (var a in list) a.Patient = null;
        return list;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var a = await _repo.FindAsync(id);
        if (a == null) return false;
        var fullPath = Path.Combine(_env.ContentRootPath, "wwwroot", a.Path.Replace('/', Path.DirectorySeparatorChar));
        if (System.IO.File.Exists(fullPath)) System.IO.File.Delete(fullPath);
        _repo.Remove(a);
        await _repo.SaveChangesAsync();
        return true;
    }
}
