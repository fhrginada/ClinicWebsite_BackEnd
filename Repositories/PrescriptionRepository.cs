using ClinicalBackend.Data;
using ClinicalBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicalBackend.Services
{
    public class PrescriptionRepository
    {
        private readonly AppDbContext _context;

        public PrescriptionRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get all prescriptions with details
        public async Task<List<Prescription>> GetAllPrescriptionsAsync()
        {
            return await _context.Prescriptions
                .Include(p => p.PrescriptionDetails) // لو عندك navigation property
                .ToListAsync();
        }

        // Get prescription by ID with details
        public async Task<Prescription?> GetPrescriptionByIdAsync(int id)
        {
            return await _context.Prescriptions
                .Include(p => p.PrescriptionDetails) // لو عندك navigation property
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // Add new prescription
        public async Task<Prescription> AddPrescriptionAsync(Prescription prescription)
        {
            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();
            return prescription;
        }

        // Update prescription
        public async Task<Prescription?> UpdatePrescriptionAsync(Prescription prescription)
        {
            var existing = await _context.Prescriptions.FindAsync(prescription.Id);
            if (existing == null) return null;

            existing.PatientName = prescription.PatientName;
            existing.MedicationName = prescription.MedicationName;
            existing.Details = prescription.Details;
            existing.Dosage = prescription.Dosage;
            existing.Instructions = prescription.Instructions;
            existing.IssueDate = prescription.IssueDate;
            existing.ExpiryDate = prescription.ExpiryDate;

            await _context.SaveChangesAsync();
            return existing;
        }

        // Delete prescription
        public async Task<bool> DeletePrescriptionAsync(int id)
        {
            var existing = await _context.Prescriptions.FindAsync(id);
            if (existing == null) return false;

            _context.Prescriptions.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
