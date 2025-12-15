using PatientApi.Models;
using Microsoft.AspNetCore.Http;

namespace PatientApi.Services.Interfaces;

public interface IPatientAttachmentService
{
    Task<PatientAttachment?> UploadAsync(int patientId, IFormFile file);
    Task<PatientAttachment?> GetAsync(int id);
    Task<IEnumerable<PatientAttachment>> ListAsync(int patientId);
    Task<bool> DeleteAsync(int id);
}
