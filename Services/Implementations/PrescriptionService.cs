using PatientApi.Models.Entities;
using PatientApi.Repositories.Interfaces;
using PatientApi.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatientApi.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IPrescriptionRepository _repository;

        public PrescriptionService(IPrescriptionRepository repository)
        {
            _repository = repository;
        }

        public Task AddAsync(Prescription prescription) => _repository.AddAsync(prescription);
        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
        public Task<IEnumerable<Prescription>> GetAllAsync() => _repository.GetAllAsync();
        public Task<Prescription?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);
        public Task<bool> UpdateAsync(Prescription prescription) => _repository.UpdateAsync(prescription);
    }
}
