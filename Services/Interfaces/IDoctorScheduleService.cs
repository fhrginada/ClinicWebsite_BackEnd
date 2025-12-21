using PatientApi.Models.Entities;
using PatientApi.Models.ViewModels;

namespace PatientApi.Services.Interfaces
{
    public interface IDoctorScheduleService
    {
        Task<IEnumerable<DoctorSchedule>> GetByDoctorIdAsync(int doctorId);
        Task AddAsync(DoctorSchedule schedule);
        Task<bool> DeleteAsync(int id);
        Task<DoctorAvailabilityResponse?> GetDoctorAvailabilityAsync(int doctorId, DateTime? startDate = null, int days = 7);
    }
}
