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
                .Include(c => c.Appointment!)
                    .ThenInclude(a => a.Patient)!
                        .ThenInclude(p => p.User)
                .Include(c => c.Appointment!)
                    .ThenInclude(a => a.Doctor)
                .FirstOrDefaultAsync(c => c.AppointmentId == appointmentId);
        }

        public async Task<Consultation?> GetByIdAsync(int consultationId)
        {
            return await _context.Consultations
                .Include(c => c.Appointment!)
                    .ThenInclude(a => a.Patient)
                .Include(c => c.Appointment!)
                    .ThenInclude(a => a.Doctor)
                .FirstOrDefaultAsync(c => c.Id == consultationId);
        }

        public async Task<bool> UpdateAsync(Consultation consultation)
        {
            var existing = await _context.Consultations.FindAsync(consultation.Id);
            if (existing == null) return false;

            existing.Symptoms = consultation.Symptoms;
            existing.Diagnosis = consultation.Diagnosis;
            existing.Prescription = consultation.Prescription;
            existing.TreatmentPlan = consultation.TreatmentPlan;
            existing.FollowUpInstructions = consultation.FollowUpInstructions;
            existing.FollowUpDate = consultation.FollowUpDate;
            existing.ConsultationFee = consultation.ConsultationFee;
            existing.Notes = consultation.Notes;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
