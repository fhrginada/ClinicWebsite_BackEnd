using System.Collections.Generic;
using System.Linq;
using ClinicBackend_Final.Data;
using ClinicBackend_Final.Models;
using ClinicBackend_Final.Repositories.Interfaces;

namespace ClinicBackend_Final.Repositories
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly AppDbContext _context;
        public PrescriptionRepository(AppDbContext context) => _context = context;

        public Prescription GetById(int id) => _context.Prescriptions.FirstOrDefault(p => p.Id == id);

        public IEnumerable<Prescription> GetByConsultationId(int consultationId) =>
            _context.Prescriptions.Where(p => p.ConsultationId == consultationId).ToList();

        public void Add(Prescription prescription)
        {
            _context.Prescriptions.Add(prescription);
            _context.SaveChanges();
        }

        public void Update(Prescription prescription)
        {
            _context.Prescriptions.Update(prescription);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _context.Prescriptions.Find(id);
            if (entity != null)
            {
                _context.Prescriptions.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}
