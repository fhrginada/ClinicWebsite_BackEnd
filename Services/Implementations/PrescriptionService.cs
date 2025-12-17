using ClinicBackend_Final.Models.Entities;
using ClinicBackend_Final.Repositories.Interfaces;
using ClinicBackend_Final.Services.Interfaces;
using ClinicBackend_Final.ViewModels.PrescriptionViewModels;

namespace ClinicBackend_Final.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IPrescriptionRepository _repo;

        public PrescriptionService(IPrescriptionRepository repo)
        {
            _repo = repo;
        }

        public PrescriptionViewModel Create(CreatePrescriptionViewModel model)
        {
            var prescription = new Prescription
            {
                ConsultationId = model.ConsultationId,
                DoctorId = model.DoctorId,
                PatientId = model.PatientId,
                Status = "Draft",
                Items = model.Items.Select(i => new PrescriptionItem
                {
                    MedicationId = i.MedicationId,
                    Dose = i.Dose,
                    Frequency = i.Frequency
                }).ToList()
            };

            _repo.Add(prescription);

            return new PrescriptionViewModel
            {
                Id = prescription.Id,
                ConsultationId = prescription.ConsultationId,
                DoctorId = prescription.DoctorId,
                PatientId = prescription.PatientId,
                Status = prescription.Status
            };
        }

        public void Confirm(int id)
        {
            var pres = _repo.GetById(id);
            if (pres != null && pres.Status == "Draft")
            {
                pres.Status = "Confirmed";
                _repo.Update(pres);
            }
        }
    }
}
