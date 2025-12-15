using PatientApi.Models.Entities;
using System.Linq;

namespace PatientApi.Repositories.Interfaces;

public interface IMedicalHistoryRepository
{
    IQueryable<MedicalHistory> Query();
    Task<MedicalHistory?> FindAsync(int id);
    Task AddAsync(MedicalHistory item);
    void Remove(MedicalHistory item);
    Task SaveChangesAsync();
}
