using PatientApi.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatientApi.Repositories.Interfaces
{
    public interface IMedicationRepository
    {
        Task<IEnumerable<Medication>> GetAllAsync();
        Task<Medication> GetByIdAsync(int id);
        Task AddAsync(Medication med);
        Task<bool> UpdateAsync(Medication med);
        Task<bool> DeleteAsync(int id);
    }
}
