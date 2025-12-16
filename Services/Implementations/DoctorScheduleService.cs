using PatientApi.Models.Entities;
using PatientApi.Repositories.Interfaces;
using PatientApi.Services.Interfaces;

namespace PatientApi.Services.Implementations
{
    public class DoctorScheduleService : IDoctorScheduleService
    {
        private readonly IDoctorScheduleRepository _repo;

        public DoctorScheduleService(IDoctorScheduleRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<DoctorSchedule>> GetByDoctorIdAsync(int doctorId)
        {
            return await _repo.GetByDoctorIdAsync(doctorId);
        }

        public async Task AddAsync(DoctorSchedule schedule)
        {
            await _repo.AddAsync(schedule);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
