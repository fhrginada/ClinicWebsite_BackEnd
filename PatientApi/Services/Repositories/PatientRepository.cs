using PatientApi.Data;
using PatientApi.Models;
using PatientApi.Services.Repositories.Interfaces;

namespace PatientApi.Services.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly AppDbContext _db;
    public PatientRepository(AppDbContext db) => _db = db;

    public IQueryable<Patient> Query() => _db.Patients;

    public Task<Patient?> FindAsync(int id) => _db.Patients.FindAsync(id).AsTask();

    public Task AddAsync(Patient patient)
    {
        _db.Patients.Add(patient);
        return Task.CompletedTask;
    }

    public void Remove(Patient patient) => _db.Patients.Remove(patient);

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
