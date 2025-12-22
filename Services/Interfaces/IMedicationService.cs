using PatientApi.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatientApi.Services.Interfaces
{
    public interface IMedicationService
    {
        Task<IEnumerable<Medication>> GetAllAsync();
        Task<Medication> GetByIdAsync(int id);
        Task AddAsync(Medication med);
        Task<bool> UpdateAsync(Medication med);
        Task<bool> DeleteAsync(int id);
    }
}