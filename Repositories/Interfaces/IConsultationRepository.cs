using PatientApi.Models.Entities;

namespace PatientApi.Repositories.Interfaces
{
    public interface IConsultationRepository
    {
        Task AddAsync(Consultation consultation);
        Task<Consultation?> GetByAppointmentIdAsync(int appointmentId);
        Task<Consultation?> GetByIdAsync(int consultationId);
        Task<bool> UpdateAsync(Consultation consultation);
    }
}
