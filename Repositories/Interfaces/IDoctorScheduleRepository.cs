using PatientApi.Models.Entities;

namespace PatientApi.Repositories.Interfaces
{
    public interface IDoctorScheduleRepository
    {
        Task<IEnumerable<DoctorSchedule>> GetByDoctorIdAsync(int doctorId);
        Task<DoctorSchedule?> GetByIdAsync(int id);
        Task AddAsync(DoctorSchedule schedule);
        Task<bool> DeleteAsync(int id);
    }
}

