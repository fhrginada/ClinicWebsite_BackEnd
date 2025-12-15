using PatientApi.Models;
using System.Linq;

namespace PatientApi.Services.Repositories.Interfaces;

public interface IPatientAttachmentRepository
{
    IQueryable<PatientAttachment> Query();
    Task<PatientAttachment?> FindAsync(int id);
    Task AddAsync(PatientAttachment item);
    void Remove(PatientAttachment item);
    Task SaveChangesAsync();
}
