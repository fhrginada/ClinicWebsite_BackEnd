using System.Collections.Generic;
using ClinicBackend_Final.Models;

namespace ClinicBackend_Final.Services.Interfaces
{
    public interface IPrescriptionService
    {
        Prescription GetById(int id);
        IEnumerable<Prescription> GetByConsultationId(int consultationId);
        void Create(Prescription prescription);
        void Confirm(int id);
    }
}
