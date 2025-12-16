using PatientApi.Models.Entities;

namespace PatientApi.Services.Interfaces
{
    public interface INurseScheduleService
    {
        Task<IEnumerable<NurseSchedule>> GetByNurseIdAsync(int nurseId);
        Task AddAsync(NurseSchedule schedule);
        Task<bool> DeleteAsync(int id);
    }
}
