using PatientApi.DTOs;
using Microsoft.AspNetCore.Http;

namespace PatientApi.Services.Interfaces;

public interface IPatientAttachmentService
{
    Task<PatientAttachmentDto?> UploadAsync(int patientId, IFormFile file);
    Task<PatientAttachmentDto?> GetAsync(int id);
    Task<IEnumerable<PatientAttachmentDto>> ListAsync(int patientId);
    Task<bool> DeleteAsync(int id);
}
