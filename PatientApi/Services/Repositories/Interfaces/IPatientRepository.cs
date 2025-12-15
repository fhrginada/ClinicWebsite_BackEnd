using PatientApi.Models;
using System.Linq;

namespace PatientApi.Services.Repositories.Interfaces;

public interface IPatientRepository
{
    IQueryable<Patient> Query();
    Task<Patient?> FindAsync(int id);
    Task AddAsync(Patient patient);
    void Remove(Patient patient);
    Task SaveChangesAsync();
}
