using Microsoft.EntityFrameworkCore;
using PatientApi.Data;
using PatientApi.Models.Entities;
using PatientApi.Repositories.Interfaces;

namespace PatientApi.Repositories.Implementations
{
    public class ConsultationRepository : IConsultationRepository
    {
        private readonly AppDbContext _context;

        public ConsultationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Consultation consultation)
        {
            await _context.Consultations.AddAsync(consultation);
            await _context.SaveChangesAsync();
        }

        public async Task<Consultation?> GetByAppointmentIdAsync(int appointmentId)
        {
            return await _context.Consultations
                .Include(c => c.Appointment)
                .FirstOrDefaultAsync(c => c.AppointmentId == appointmentId);
        }
    }
}
