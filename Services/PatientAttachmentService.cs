using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PatientApi.Data;
using PatientApi.Models.Entities;
using PatientApi.Services.Interfaces;
using PatientApi.Repositories.Interfaces;
using PatientApi.Models.ViewModels;

namespace PatientApi.Services;

public class PatientAttachmentService : IPatientAttachmentService
{
    private readonly IPatientAttachmentRepository _repo;
    private readonly IPatientRepository _patientRepo;
    private readonly IWebHostEnvironment _env;
    private readonly string _baseDir;

    public PatientAttachmentService(IPatientAttachmentRepository repo, IPatientRepository patientRepo, IWebHostEnvironment env)
    {
        _repo = repo;
        _patientRepo = patientRepo;
        _env = env;
        _baseDir = Path.Combine(_env.ContentRootPath, "wwwroot", "uploads", "patients");
        Directory.CreateDirectory(_baseDir);
    }

    public async Task<PatientAttachmentViewModel?> UploadAsync(int patientId, IFormFile file)
    {
        var patient = await _patientRepo.FindAsync(patientId);
        if (patient == null) return null;
        var patientDir = Path.Combine(_baseDir, patientId.ToString());
        Directory.CreateDirectory(patientDir);
        var fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
        var relPath = Path.Combine("uploads", "patients", patientId.ToString(), fileName).Replace("\\", "/");
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
        return ViewModelMapper.ToViewModel(att, "");
    }

    public async Task<PatientAttachmentViewModel?> GetAsync(int id)
    {
        var a = await _repo.FindAsync(id);
        if (a == null) return null;
        a.Patient = null;
        return ViewModelMapper.ToViewModel(a, "");
    }

    public async Task<IEnumerable<PatientAttachmentViewModel>> ListAsync(int patientId)
    {
        var list = await _repo.Query().Where(x => x.PatientId == patientId).OrderByDescending(x => x.UploadedAt).ToListAsync();
        foreach (var a in list) a.Patient = null;
        return list.Select(x => ViewModelMapper.ToViewModel(x, ""));
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
