using ClinicalBackend.Data;
using ClinicalBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicalBackend.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly AppDbContext _context;

        public PrescriptionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Prescription>> GetAllAsync()
        {
            return await _context.Prescriptions.ToListAsync();
        }

        public async Task<Prescription?> GetByIdAsync(int id)
        {
            return await _context.Prescriptions.FindAsync(id);
        }

        public async Task<Prescription> CreateAsync(Prescription prescription)
        {
            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();
            return prescription;
        }
    }
}