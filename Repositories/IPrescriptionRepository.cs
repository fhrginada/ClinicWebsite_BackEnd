using ClinicalBackend.Models;

namespace ClinicalBackend.Repositories
{
    public interface IPrescriptionRepository
    {
        Task<List<Prescription>> GetAllAsync();
        Task<Prescription?> GetByIdAsync(int id);
        Task AddAsync(Prescription prescription);
    }
}
