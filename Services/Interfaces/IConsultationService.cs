using PatientApi.Models.ViewModels;

namespace PatientApi.Services.Interfaces
{
    public interface IConsultationService
    {
        Task<bool> CreateAsync(ConsultationRequest request);
    }
}
