using PatientApi.Models.Entities;
using PatientApi.Repositories.Interfaces;
using PatientApi.Services.Interfaces;

namespace PatientApi.Services.Implementations
{
    public class NurseScheduleService : INurseScheduleService
    {
        private readonly INurseScheduleRepository _repo;

        public NurseScheduleService(INurseScheduleRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<NurseSchedule>> GetByNurseIdAsync(int nurseId)
        {
            return await _repo.GetByNurseIdAsync(nurseId);
        }

        public async Task AddAsync(NurseSchedule schedule)
        {
            await _repo.AddAsync(schedule);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
