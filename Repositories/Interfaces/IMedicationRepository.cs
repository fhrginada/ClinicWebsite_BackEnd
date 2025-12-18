using System.Collections.Generic;
using ClinicBackend_Final.Models;

namespace ClinicBackend_Final.Repositories.Interfaces
{
    public interface IMedicationRepository
    {
        IEnumerable<Medication> GetAll();
        Medication GetById(int id);
        void Add(Medication medication);
        void Update(Medication medication);
        void Delete(int id);
    }
}
