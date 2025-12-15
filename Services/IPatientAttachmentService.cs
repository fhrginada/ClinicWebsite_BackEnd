using PatientApi.Models.Entities;
using PatientApi.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace PatientApi.Services.Interfaces;

public interface IPatientAttachmentService
{
    Task<PatientAttachmentViewModel?> UploadAsync(int patientId, IFormFile file);
    Task<PatientAttachmentViewModel?> GetAsync(int id);
    Task<IEnumerable<PatientAttachmentViewModel>> ListAsync(int patientId);
    Task<bool> DeleteAsync(int id);
}
