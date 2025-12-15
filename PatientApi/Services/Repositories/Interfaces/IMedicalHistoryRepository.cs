using PatientApi.Models;
using System.Linq;

namespace PatientApi.Services.Repositories.Interfaces;

public interface IMedicalHistoryRepository
{
    IQueryable<MedicalHistory> Query();
    Task<MedicalHistory?> FindAsync(int id);
    Task AddAsync(MedicalHistory item);
    void Remove(MedicalHistory item);
    Task SaveChangesAsync();
}
