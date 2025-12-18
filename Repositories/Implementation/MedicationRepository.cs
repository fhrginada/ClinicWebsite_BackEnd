using System.Collections.Generic;
using System.Linq;
using ClinicBackend_Final.Data;
using ClinicBackend_Final.Models;
using ClinicBackend_Final.Repositories.Interfaces;

namespace ClinicBackend_Final.Repositories
{
    public class MedicationRepository : IMedicationRepository
    {
        private readonly AppDbContext _context;
        public MedicationRepository(AppDbContext context) => _context = context;

        public IEnumerable<Medication> GetAll() => _context.Medications.ToList();
        public Medication GetById(int id) => _context.Medications.Find(id);
        public void Add(Medication medication)
        {
            _context.Medications.Add(medication);
            _context.SaveChanges();
        }
        public void Update(Medication medication)
        {
            _context.Medications.Update(medication);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var entity = _context.Medications.Find(id);
            if (entity != null)
            {
                _context.Medications.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}
