using Microsoft.EntityFrameworkCore;
using PatientApi.Data;
using PatientApi.DTOs;
using PatientApi.Models;
using PatientApi.Services.Interfaces;

namespace PatientApi.Services;

public class PatientService : IPatientService
{
    private readonly AppDbContext _db;
    public PatientService(AppDbContext db) => _db = db;

    public async Task<(int total, List<PatientDto> items)> GetAsync(string? name, int? gender, int? ageMin, int? ageMax, int page, int pageSize)
    {
        var q = _db.Patients.AsQueryable();
        if (!string.IsNullOrWhiteSpace(name)) q = q.Where(p => p.FirstName.Contains(name) || (p.LastName != null && p.LastName.Contains(name)));
        if (gender.HasValue) q = q.Where(p => (int)p.Gender == gender.Value);
        if (ageMin.HasValue || ageMax.HasValue)
        {
            var today = DateTime.UtcNow;
            if (ageMin.HasValue) q = q.Where(p => p.DateOfBirth <= today.AddYears(-ageMin.Value));
            if (ageMax.HasValue) q = q.Where(p => p.DateOfBirth >= today.AddYears(-ageMax.Value - 1).AddDays(1));
        }

        var total = await q.CountAsync();
        var list = await q.OrderBy(p => p.Id).Skip((page - 1) * pageSize).Take(pageSize).Include(p => p.MedicalHistories).ToListAsync();

        var items = list.Select(p => new PatientDto
        {
            Id = p.Id,
            UserId = p.UserId,
            FirstName = p.FirstName,
            LastName = p.LastName,
            DateOfBirth = p.DateOfBirth,
            Gender = p.Gender,
            Phone = p.Phone,
            Email = p.Email,
            CreatedAt = p.CreatedAt,
            MedicalHistories = p.MedicalHistories.Select(m => new MedicalHistoryDto
            {
                Id = m.Id,
                PatientId = m.PatientId,
                Date = m.Date,
                Diagnosis = m.Diagnosis,
                Treatment = m.Treatment,
                Notes = m.Notes,
                CreatedAt = m.CreatedAt
            }).ToList()
        }).ToList();

        return (total, items);
    }

    public async Task<PatientDto?> GetByIdAsync(int id)
    {
        var p = await _db.Patients.Include(x => x.MedicalHistories).FirstOrDefaultAsync(x => x.Id == id);
        if (p == null) return null;
        return new PatientDto
        {
            Id = p.Id,
            UserId = p.UserId,
            FirstName = p.FirstName,
            LastName = p.LastName,
            DateOfBirth = p.DateOfBirth,
            Gender = p.Gender,
            Phone = p.Phone,
            Email = p.Email,
            CreatedAt = p.CreatedAt,
            MedicalHistories = p.MedicalHistories.Select(m => new MedicalHistoryDto
            {
                Id = m.Id,
                PatientId = m.PatientId,
                Date = m.Date,
                Diagnosis = m.Diagnosis,
                Treatment = m.Treatment,
                Notes = m.Notes,
                CreatedAt = m.CreatedAt
            }).ToList()
        };
    }

    public async Task<PatientDto> CreateAsync(PatientCreateDto dto)
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
        _db.Patients.Add(p);
        await _db.SaveChangesAsync();
        return new PatientDto
        {
            Id = p.Id,
            UserId = p.UserId,
            FirstName = p.FirstName,
            LastName = p.LastName,
            DateOfBirth = p.DateOfBirth,
            Gender = p.Gender,
            Phone = p.Phone,
            Email = p.Email,
            CreatedAt = p.CreatedAt,
            MedicalHistories = new List<MedicalHistoryDto>()
        };
    }

    public async Task<bool> UpdateAsync(int id, PatientUpdateDto dto)
    {
        var p = await _db.Patients.FindAsync(id);
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
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var p = await _db.Patients.FindAsync(id);
        if (p == null) return false;
        _db.Patients.Remove(p);
        await _db.SaveChangesAsync();
        return true;
    }
}
