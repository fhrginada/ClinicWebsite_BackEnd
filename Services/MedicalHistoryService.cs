using Microsoft.EntityFrameworkCore;
using PatientApi.Data;
using PatientApi.Models.Entities;
using PatientApi.Services.Interfaces;
using PatientApi.Repositories.Interfaces;
using PatientApi.Models.ViewModels;

namespace PatientApi.Services;

public class MedicalHistoryService : IMedicalHistoryService
{
    private readonly IMedicalHistoryRepository _repo;
    private readonly IPatientRepository _patientRepo;
    public MedicalHistoryService(IMedicalHistoryRepository repo, IPatientRepository patientRepo)
    {
        _repo = repo;
        _patientRepo = patientRepo;
    }

    public async Task<(int total, List<MedicalHistoryViewModel> items)> GetAsync(int? patientId, int page, int pageSize)
    {
        var q = _repo.Query();
        if (patientId.HasValue) q = q.Where(m => m.PatientId == patientId.Value);
        var total = await q.CountAsync();
        var list = await q.OrderByDescending(m => m.DateRecorded).Skip((page - 1) * pageSize).Take(pageSize).Include(m => m.Patient).ToListAsync();
        var vm = list.Select(ViewModelMapper.ToViewModel).ToList();
        return (total, vm);
    }

    public async Task<MedicalHistoryViewModel?> GetByIdAsync(int id)
    {
        var m = await _repo.Query().Include(x => x.Patient).FirstOrDefaultAsync(x => x.Id == id);
        if (m == null) return null;
        return ViewModelMapper.ToViewModel(m);
    }

    public async Task<MedicalHistoryViewModel?> CreateAsync(MedicalHistory dto)
    {
        var patient = await _patientRepo.FindAsync(dto.PatientId);
        if (patient == null) return null;
        var m = new MedicalHistory
        {
            PatientId = dto.PatientId,
            DoctorId = dto.DoctorId,
            DateRecorded = dto.DateRecorded,
            FollowUpDate = dto.FollowUpDate,
            Diagnosis = dto.Diagnosis,
            Treatment = dto.Treatment,
            AttachmentUrl = dto.AttachmentUrl
        };
        await _repo.AddAsync(m);
        await _repo.SaveChangesAsync();
        return ViewModelMapper.ToViewModel(m);
    }

    public async Task<bool> UpdateAsync(int id, MedicalHistory dto)
    {
        var m = await _repo.FindAsync(id);
        if (m == null) return false;
        m.DoctorId = dto.DoctorId;
        m.DateRecorded = dto.DateRecorded;
        m.FollowUpDate = dto.FollowUpDate;
        m.Diagnosis = dto.Diagnosis;
        m.Treatment = dto.Treatment;
        m.AttachmentUrl = dto.AttachmentUrl;
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
