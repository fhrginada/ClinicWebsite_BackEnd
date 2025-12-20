using PatientApi.Models.ViewModels;

namespace PatientApi.Services.Interfaces
{
    public interface IConsultationService
    {
        Task<bool> CreateAsync(ConsultationRequest request);
        Task<ConsultationResponse?> GetByAppointmentIdAsync(int appointmentId);
        Task<bool> UpdateAsync(int consultationId, ConsultationRequest request);
    }
}
