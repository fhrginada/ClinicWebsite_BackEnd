using PatientApi.Models.Entities;
using System.Linq;

namespace PatientApi.Repositories.Interfaces;

public interface IPatientRepository
{
    IQueryable<Patient> Query();
    Task<Patient?> FindAsync(int id);
    Task AddAsync(Patient patient);
    void Remove(Patient patient);
    Task SaveChangesAsync();
}
