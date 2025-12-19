using PatientApi.Models.ViewModels;

namespace PatientApi.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<int> CreateAsync(AppointmentRequest request);
        Task<bool> UpdateStatusAsync(AppointmentStatusRequest request);
        Task<IEnumerable<AppointmentResponse>> GetAllAsync();
    }
}
