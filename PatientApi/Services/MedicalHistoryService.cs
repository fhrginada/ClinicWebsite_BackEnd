using Microsoft.EntityFrameworkCore;
using PatientApi.Data;
using PatientApi.Models;
using PatientApi.Services.Interfaces;

namespace PatientApi.Services;

public class MedicalHistoryService : IMedicalHistoryService
{
    private readonly PatientApi.Services.Repositories.Interfaces.IMedicalHistoryRepository _repo;
    private readonly PatientApi.Services.Repositories.Interfaces.IPatientRepository _patientRepo;
    public MedicalHistoryService(PatientApi.Services.Repositories.Interfaces.IMedicalHistoryRepository repo, PatientApi.Services.Repositories.Interfaces.IPatientRepository patientRepo)
    {
        _repo = repo;
        _patientRepo = patientRepo;
    }

    public async Task<(int total, List<MedicalHistory> items)> GetAsync(int? patientId, int page, int pageSize)
    {
        var q = _repo.Query();
        if (patientId.HasValue) q = q.Where(m => m.PatientId == patientId.Value);
        var total = await q.CountAsync();
        var list = await q.OrderByDescending(m => m.Date).Skip((page - 1) * pageSize).Take(pageSize).Include(m => m.Patient).ToListAsync();

        // remove back-reference to avoid JSON cycles
        foreach (var m in list)
        {
            m.Patient = null;
        }

        return (total, list);
    }

    public async Task<MedicalHistory?> GetByIdAsync(int id)
    {
        var m = await _repo.Query().Include(x => x.Patient).FirstOrDefaultAsync(x => x.Id == id);
        if (m == null) return null;
        m.Patient = null;
        return m;
    }

    public async Task<MedicalHistory?> CreateAsync(MedicalHistory dto)
    {
        var patient = await _patientRepo.FindAsync(dto.PatientId);
        if (patient == null) return null;
        var m = new MedicalHistory
        {
            PatientId = dto.PatientId,
            Date = dto.Date,
            Diagnosis = dto.Diagnosis,
            Treatment = dto.Treatment,
            Notes = dto.Notes
        };
        await _repo.AddAsync(m);
        await _repo.SaveChangesAsync();
        m.Patient = null;
        return m;
    }

    public async Task<bool> UpdateAsync(int id, MedicalHistory dto)
    {
        var m = await _repo.FindAsync(id);
        if (m == null) return false;
        m.Date = dto.Date;
        m.Diagnosis = dto.Diagnosis;
        m.Treatment = dto.Treatment;
        m.Notes = dto.Notes;
        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var m = await _repo.FindAsync(id);
        if (m == null) return false;
        _repo.Remove(m);
        await _repo.SaveChangesAsync();
        return true;
    }
}
