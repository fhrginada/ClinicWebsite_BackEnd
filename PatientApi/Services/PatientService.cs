using Microsoft.EntityFrameworkCore;
using PatientApi.Data;
using PatientApi.Models;
using PatientApi.Services.Interfaces;

namespace PatientApi.Services;

public class PatientService : IPatientService
{
    private readonly PatientApi.Services.Repositories.Interfaces.IPatientRepository _repo;
    public PatientService(PatientApi.Services.Repositories.Interfaces.IPatientRepository repo) => _repo = repo;

    public async Task<(int total, List<Patient> items)> GetAsync(string? name, int? gender, int? ageMin, int? ageMax, int page, int pageSize)
    {
        var q = _repo.Query();
        if (!string.IsNullOrWhiteSpace(name)) q = q.Where(p => p.FirstName.Contains(name) || (p.LastName != null && p.LastName.Contains(name)));
        if (gender.HasValue) q = q.Where(p => (int)p.Gender == gender.Value);
        if (ageMin.HasValue || ageMax.HasValue)
        {
            var today = DateTime.UtcNow;
            if (ageMin.HasValue) q = q.Where(p => p.DateOfBirth <= today.AddYears(-ageMin.Value));
            if (ageMax.HasValue) q = q.Where(p => p.DateOfBirth >= today.AddYears(-ageMax.Value - 1).AddDays(1));
        }

        var total = await q.CountAsync();
        var list = await q.OrderBy(p => p.Id).Skip((page - 1) * pageSize).Take(pageSize).Include(p => p.MedicalHistories).Include(p => p.Attachments).ToListAsync();

        // remove back-references to avoid JSON cycles
        foreach (var p in list)
        {
            foreach (var m in p.MedicalHistories) m.Patient = null;
            foreach (var a in p.Attachments) a.Patient = null;
        }

        return (total, list);
    }

    public async Task<Patient?> GetByIdAsync(int id)
    {
        var p = await _repo.Query().Include(x => x.MedicalHistories).Include(x => x.Attachments).FirstOrDefaultAsync(x => x.Id == id);
        if (p == null) return null;
        foreach (var m in p.MedicalHistories) m.Patient = null;
        foreach (var a in p.Attachments) a.Patient = null;
        return p;
    }

    public async Task<Patient> CreateAsync(Patient dto)
    {
        // Domain validation
        var today = DateTime.UtcNow.Date;
        if (dto.DateOfBirth >= today) throw new ArgumentException("DateOfBirth must be in the past");
        var age = today.Year - dto.DateOfBirth.Year - (dto.DateOfBirth.Date > today.AddYears(- (today.Year - dto.DateOfBirth.Year)) ? 1 : 0);
        if (age <= 0 || age > 150) throw new ArgumentException("Invalid age");
        if (dto.Gender == PatientApi.Models.Gender.Unknown) throw new ArgumentException("Gender must be Male, Female or Other");

        var p = new Patient
        {
            UserId = dto.UserId,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            DateOfBirth = dto.DateOfBirth,
            Gender = dto.Gender,
            Phone = dto.Phone,
            Email = dto.Email,
        };
        await _repo.AddAsync(p);
        await _repo.SaveChangesAsync();

        p.MedicalHistories = new List<MedicalHistory>();
        p.Attachments = new List<PatientAttachment>();
        return p;
    }

    public async Task<bool> UpdateAsync(int id, Patient dto)
    {
        var p = await _repo.FindAsync(id);
        if (p == null) return false;
        // Domain validation
        var today = DateTime.UtcNow.Date;
        if (dto.DateOfBirth >= today) throw new ArgumentException("DateOfBirth must be in the past");
        var age = today.Year - dto.DateOfBirth.Year - (dto.DateOfBirth.Date > today.AddYears(- (today.Year - dto.DateOfBirth.Year)) ? 1 : 0);
        if (age <= 0 || age > 150) throw new ArgumentException("Invalid age");
        if (dto.Gender == PatientApi.Models.Gender.Unknown) throw new ArgumentException("Gender must be Male, Female or Other");

        p.FirstName = dto.FirstName;
        p.LastName = dto.LastName;
        p.DateOfBirth = dto.DateOfBirth;
        p.Gender = dto.Gender;
        p.Phone = dto.Phone;
        p.Email = dto.Email;
        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var p = await _repo.FindAsync(id);
        if (p == null) return false;
        _repo.Remove(p);
        await _repo.SaveChangesAsync();
        return true;
    }
}
