using PatientApi.Models.Entities;

namespace PatientApi.Repositories.Interfaces
{
    public interface INurseRepository
    {
        Task<IEnumerable<Nurse>> GetAllAsync();
        Task<Nurse> GetByIdAsync(int id);
        Task AddAsync(Nurse nurse);
        Task<bool> UpdateAsync(Nurse nurse);
        Task<bool> DeleteAsync(int id);
    }
}
