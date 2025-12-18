using PatientApi.Models.Entities;
using PatientApi.Models.ViewModels;

namespace PatientApi.Services.Interfaces;

public interface IMedicalHistoryService
{
    Task<(int total, List<MedicalHistoryViewModel> items)> GetAsync(int? patientId, int page, int pageSize);
    Task<MedicalHistoryViewModel?> GetByIdAsync(int id);
    Task<MedicalHistoryViewModel?> CreateAsync(MedicalHistory dto);
    Task<bool> UpdateAsync(int id, MedicalHistory dto);
    Task<bool> DeleteAsync(int id);
}
