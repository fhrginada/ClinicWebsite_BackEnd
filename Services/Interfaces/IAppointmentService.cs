using PatientApi.Models.ViewModels;

namespace PatientApi.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<int> CreateAsync(int patientId, AppointmentRequest request);
        Task<bool> UpdateStatusAsync(AppointmentStatusRequest request, int? actingDoctorId = null);
        Task<IEnumerable<AppointmentResponse>> GetAllAsync();
        Task<IEnumerable<AppointmentResponse>> GetForPatientAsync(int patientId);
        Task<IEnumerable<AppointmentResponse>> GetForDoctorAsync(int doctorId);
    }
}
