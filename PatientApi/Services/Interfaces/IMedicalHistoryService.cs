using PatientApi.Models;

namespace PatientApi.Services.Interfaces;

public interface IMedicalHistoryService
{
    Task<(int total, List<MedicalHistory> items)> GetAsync(int? patientId, int page, int pageSize);
    Task<MedicalHistory?> GetByIdAsync(int id);
    Task<MedicalHistory?> CreateAsync(MedicalHistory dto);
    Task<bool> UpdateAsync(int id, MedicalHistory dto);
    Task<bool> DeleteAsync(int id);
}
