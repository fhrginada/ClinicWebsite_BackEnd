using PatientApi.Models.Entities;
using PatientApi.Models.ViewModels;
using PatientApi.Repositories.Interfaces;
using PatientApi.Services.Interfaces;

namespace PatientApi.Services.Implementations
{
    public class ConsultationService : IConsultationService
    {
        private readonly IConsultationRepository _repo;

        public ConsultationService(IConsultationRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> CreateAsync(ConsultationRequest request)
        {
            var consultation = new Consultation
            {
                AppointmentId = request.AppointmentId!.Value,
                Symptoms = request.Symptoms,
                Diagnosis = request.Diagnosis,
                Prescription = request.Prescription,
                TreatmentPlan = request.TreatmentPlan,
                FollowUpInstructions = request.FollowUpInstructions,
                FollowUpDate = request.FollowUpDate,
                ConsultationFee = request.ConsultationFee,
                Notes = request.Notes
            };

            await _repo.AddAsync(consultation);
            return true;
        }
    }
}
