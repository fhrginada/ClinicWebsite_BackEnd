using PatientApi.Models.Entities;
using PatientApi.Services.Interfaces;
using PatientApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatientApi.Services
{
    public class MedicationService : IMedicationService
    {
        private readonly IMedicationRepository _repo;

        public MedicationService(IMedicationRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Medication>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<Medication> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task AddAsync(Medication med) => await _repo.AddAsync(med);

        public async Task<bool> UpdateAsync(Medication med) => await _repo.UpdateAsync(med);

        public async Task<bool> DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
}