using PatientApi.Models.Entities;

namespace PatientApi.Services.Interfaces
{
    public interface IDoctorScheduleService
    {
        Task<IEnumerable<DoctorSchedule>> GetByDoctorIdAsync(int doctorId);
        Task AddAsync(DoctorSchedule schedule);
        Task<bool> DeleteAsync(int id);
    }
}
