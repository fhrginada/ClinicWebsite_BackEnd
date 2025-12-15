using PatientApi.Models;

namespace PatientApi.Services.Interfaces;

public interface IPatientService
{
    Task<(int total, List<Patient> items)> GetAsync(string? name, int? gender, int? ageMin, int? ageMax, int page, int pageSize);
    Task<Patient?> GetByIdAsync(int id);
    Task<Patient> CreateAsync(Patient dto);
    Task<bool> UpdateAsync(int id, Patient dto);
    Task<bool> DeleteAsync(int id);
}
