using PatientApi.Data;
using PatientApi.Models.Entities;
using PatientApi.Repositories.Interfaces;

namespace PatientApi.Repositories;

public class MedicalHistoryRepository : IMedicalHistoryRepository
{
    private readonly AppDbContext _db;
    public MedicalHistoryRepository(AppDbContext db) => _db = db;

    public IQueryable<MedicalHistory> Query() => _db.MedicalHistories;

    public Task<MedicalHistory?> FindAsync(int id) => _db.MedicalHistories.FindAsync(id).AsTask();

    public Task AddAsync(MedicalHistory item)
    {
        _db.MedicalHistories.Add(item);
        return Task.CompletedTask;
    }

    public void Remove(MedicalHistory item) => _db.MedicalHistories.Remove(item);

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
