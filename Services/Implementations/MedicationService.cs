using System.Collections.Generic;
using ClinicBackend_Final.Models;
using ClinicBackend_Final.Repositories.Interfaces;
using ClinicBackend_Final.Services.Interfaces;

namespace ClinicBackend_Final.Services
{
    public class MedicationService : IMedicationService
    {
        private readonly IMedicationRepository _repo;
        public MedicationService(IMedicationRepository repo) => _repo = repo;

        public IEnumerable<Medication> GetAll() => _repo.GetAll();
    }
}
