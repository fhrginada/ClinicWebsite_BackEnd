using Microsoft.EntityFrameworkCore;
using PatientApi.Data;
using PatientApi.Models.Entities;
using PatientApi.Repositories.Interfaces;

namespace PatientApi.Repositories.Implementations
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _context;

        public AppointmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .ToListAsync();
        }

        public async Task<Appointment?> GetByIdAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.Consultation)
                .FirstOrDefaultAsync(a => a.AppointmentId == id);
        }

        public async Task AddAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
