using PatientApi.Models.Entities;

namespace PatientApi.Repositories.Interfaces
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<Doctor>> GetAllAsync();
        Task<Doctor> GetByIdAsync(int id);
        Task AddAsync(Doctor doctor);
        Task<bool> UpdateAsync(Doctor doctor);
        Task<bool> DeleteAsync(int id);
    }
}
