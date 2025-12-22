using PatientApi.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatientApi.Services.Interfaces
{
    public interface IPrescriptionService
    {
        Task<IEnumerable<Prescription>> GetAllAsync();
        Task<Prescription> GetByIdAsync(int id);
        Task AddAsync(Prescription prescription);
        Task<bool> UpdateAsync(Prescription prescription);
        Task<bool> DeleteAsync(int id);
    }
}
