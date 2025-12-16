using PatientApi.Models.Entities;

namespace PatientApi.Repositories.Interfaces
{
    public interface INurseScheduleRepository
    {
        Task<IEnumerable<NurseSchedule>> GetByNurseIdAsync(int nurseId);
        Task AddAsync(NurseSchedule schedule);
        Task<bool> DeleteAsync(int id);
    }
}
