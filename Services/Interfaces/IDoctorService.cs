using PatientApi.Models.ViewModels.DoctorVM;

namespace PatientApi.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<DoctorResponse> CreateDoctorAsync(CreateDoctorRequest model);
        Task<IEnumerable<DoctorResponse>> GetAllDoctorsAsync();
        Task<DoctorResponse?> GetDoctorByIdAsync(int id);
        Task<bool> UpdateDoctorAsync(int id, UpdateDoctorRequest model);
        Task<bool> DeleteDoctorAsync(int id);
    }
}
