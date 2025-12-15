using PatientApi.DTOs;

namespace PatientApi.Services.Interfaces;

public interface IPatientService
{
    Task<(int total, List<PatientDto> items)> GetAsync(string? name, int? gender, int? ageMin, int? ageMax, int page, int pageSize);
    Task<PatientDto?> GetByIdAsync(int id);
    Task<PatientDto> CreateAsync(PatientCreateDto dto);
    Task<bool> UpdateAsync(int id, PatientUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
