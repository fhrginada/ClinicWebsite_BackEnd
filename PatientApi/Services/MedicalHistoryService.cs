using Microsoft.EntityFrameworkCore;
using PatientApi.Data;
using PatientApi.DTOs;
using PatientApi.Models;
using PatientApi.Services.Interfaces;

namespace PatientApi.Services;

public class MedicalHistoryService : IMedicalHistoryService
{
    private readonly AppDbContext _db;
    public MedicalHistoryService(AppDbContext db) => _db = db;

    public async Task<(int total, List<MedicalHistoryDto> items)> GetAsync(int? patientId, int page, int pageSize)
    {
        var q = _db.MedicalHistories.AsQueryable();
        if (patientId.HasValue) q = q.Where(m => m.PatientId == patientId.Value);
        var total = await q.CountAsync();
        var list = await q.OrderByDescending(m => m.Date).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        var items = list.Select(m => new MedicalHistoryDto
        {
            Id = m.Id,
            PatientId = m.PatientId,
            Date = m.Date,
            Diagnosis = m.Diagnosis,
            Treatment = m.Treatment,
            Notes = m.Notes,
            CreatedAt = m.CreatedAt
        }).ToList();
        return (total, items);
    }

    public async Task<MedicalHistoryDto?> GetByIdAsync(int id)
    {
        var m = await _db.MedicalHistories.FindAsync(id);
        if (m == null) return null;
        return new MedicalHistoryDto
        {
            Id = m.Id,
            PatientId = m.PatientId,
            Date = m.Date,
            Diagnosis = m.Diagnosis,
            Treatment = m.Treatment,
            Notes = m.Notes,
            CreatedAt = m.CreatedAt
        };
    }

    public async Task<MedicalHistoryDto?> CreateAsync(MedicalHistoryCreateDto dto)
    {
        var patient = await _db.Patients.FindAsync(dto.PatientId);
        if (patient == null) return null;
        var m = new MedicalHistory
        {
            PatientId = dto.PatientId,
            Date = dto.Date,
            Diagnosis = dto.Diagnosis,
            Treatment = dto.Treatment,
            Notes = dto.Notes
        };
        _db.MedicalHistories.Add(m);
        await _db.SaveChangesAsync();
        return new MedicalHistoryDto
        {
            Id = m.Id,
            PatientId = m.PatientId,
            Date = m.Date,
            Diagnosis = m.Diagnosis,
            Treatment = m.Treatment,
            Notes = m.Notes,
            CreatedAt = m.CreatedAt
        };
    }

    public async Task<bool> UpdateAsync(int id, MedicalHistoryUpdateDto dto)
    {
        var m = await _db.MedicalHistories.FindAsync(id);
        if (m == null) return false;
        m.Date = dto.Date;
        m.Diagnosis = dto.Diagnosis;
        m.Treatment = dto.Treatment;
        m.Notes = dto.Notes;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var m = await _db.MedicalHistories.FindAsync(id);
        if (m == null) return false;
        _db.MedicalHistories.Remove(m);
        await _db.SaveChangesAsync();
        return true;
    }
}
