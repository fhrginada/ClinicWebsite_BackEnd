using System.Collections.Generic;
using ClinicBackend_Final.Models;

namespace ClinicBackend_Final.Services.Interfaces
{
    public interface IMedicationService
    {
        IEnumerable<Medication> GetAll();
    }
}
