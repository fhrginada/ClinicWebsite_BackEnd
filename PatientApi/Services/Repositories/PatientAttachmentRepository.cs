using PatientApi.Data;
using PatientApi.Models;
using PatientApi.Services.Repositories.Interfaces;

namespace PatientApi.Services.Repositories;

public class PatientAttachmentRepository : IPatientAttachmentRepository
{
    private readonly AppDbContext _db;
    public PatientAttachmentRepository(AppDbContext db) => _db = db;

    public IQueryable<PatientAttachment> Query() => _db.PatientAttachments;

    public Task<PatientAttachment?> FindAsync(int id) => _db.PatientAttachments.FindAsync(id).AsTask();

    public Task AddAsync(PatientAttachment item)
    {
        _db.PatientAttachments.Add(item);
        return Task.CompletedTask;
    }

    public void Remove(PatientAttachment item) => _db.PatientAttachments.Remove(item);

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
