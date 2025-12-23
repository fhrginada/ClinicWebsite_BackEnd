using PatientApi.Data;
using PatientApi.Models.Entities;
using PatientApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatientApi.Repositories.Implementation
{
    public class MedicationRepository : IMedicationRepository
    {
        private readonly AppDbContext _context;

        public MedicationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Medication>> GetAllAsync()
        {
            return await _context.Medications.ToListAsync();
        }

        public async Task<Medication> GetByIdAsync(int id)
        {
            return await _context.Medications.FindAsync(id);
        }

        public async Task AddAsync(Medication med)
        {
            await _context.Medications.AddAsync(med);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Medication med)
        {
            _context.Medications.Update(med);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var med = await _context.Medications.FindAsync(id);
            if (med == null) return false;
            _context.Medications.Remove(med);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
