using PatientApi.Models.Entities;
using PatientApi.Models.ViewModels;

namespace PatientApi.Services.Interfaces;

public interface IPatientService
{
    Task<(int total, List<PatientViewModel> items)> GetAsync(int? gender, int page, int pageSize);
    Task<PatientViewModel?> GetByIdAsync(int id);

    // ✅ جديد – يرجّع Entity
    Task<Patient?> GetEntityByIdAsync(int id);

    Task<PatientViewModel> CreateAsync(Patient dto);
    Task<bool> UpdateAsync(int id, Patient dto);
    Task<bool> DeleteAsync(int id);
    Task<PatientDashboardResponse?> GetDashboardAsync(int patientId);
}
