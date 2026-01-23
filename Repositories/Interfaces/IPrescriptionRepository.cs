using PatientApi.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatientApi.Repositories.Interfaces
{
    public interface IPrescriptionRepository
    {
        Task<IEnumerable<Prescription>> GetAllAsync();
        Task<IEnumerable<Prescription>> GetByDoctorIdAsync(int doctorId);
        Task<IEnumerable<Prescription>> GetByPatientIdAsync(int patientId);
        Task<Prescription> GetByIdAsync(int id);
        Task AddAsync(Prescription prescription);
        Task<bool> UpdateAsync(Prescription prescription);
        Task<bool> DeleteAsync(int id);
    }
}
