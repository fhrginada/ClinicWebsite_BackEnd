using System.Collections.Generic;
using ClinicBackend_Final.Models;

namespace ClinicBackend_Final.Repositories.Interfaces
{
    public interface IPrescriptionRepository
    {
        Prescription GetById(int id);
        IEnumerable<Prescription> GetByConsultationId(int consultationId);
        void Add(Prescription prescription);
        void Update(Prescription prescription);
        void Delete(int id);
    }
}
