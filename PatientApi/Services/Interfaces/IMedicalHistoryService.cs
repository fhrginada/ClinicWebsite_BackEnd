using PatientApi.DTOs;

namespace PatientApi.Services.Interfaces;

public interface IMedicalHistoryService
{
    Task<(int total, List<MedicalHistoryDto> items)> GetAsync(int? patientId, int page, int pageSize);
    Task<MedicalHistoryDto?> GetByIdAsync(int id);
    Task<MedicalHistoryDto?> CreateAsync(MedicalHistoryCreateDto dto);
    Task<bool> UpdateAsync(int id, MedicalHistoryUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
