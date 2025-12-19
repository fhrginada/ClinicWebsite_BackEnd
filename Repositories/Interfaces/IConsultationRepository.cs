using PatientApi.Models.Entities;

namespace PatientApi.Repositories.Interfaces
{
    public interface IConsultationRepository
    {
        Task AddAsync(Consultation consultation);
        Task<Consultation?> GetByAppointmentIdAsync(int appointmentId);
    }
}
