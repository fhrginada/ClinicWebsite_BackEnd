using PatientApi.Data;
using PatientApi.Models.Entities;
using PatientApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace PatientApi.Repositories.Implementation
{
    public class DoctorScheduleRepository : IDoctorScheduleRepository
    {
        private readonly AppDbContext _context;

        public DoctorScheduleRepository (AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DoctorSchedule>> GetByDoctorIdAsync(int doctorId)
        {
            return await _context.DoctorSchedules
                .Where(s => s.DoctorId == doctorId)
                .ToListAsync();
        }

        public async Task<DoctorSchedule?> GetByIdAsync(int id)
        {
            return await _context.DoctorSchedules.FindAsync(id);
        }

        public async Task AddAsync(DoctorSchedule schedule)
        {
            await _context.DoctorSchedules.AddAsync(schedule);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var schedule = await GetByIdAsync(id);
            if (schedule == null) return false;

            _context.DoctorSchedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
