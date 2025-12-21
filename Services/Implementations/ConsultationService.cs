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

        public async Task<ConsultationResponse?> GetByAppointmentIdAsync(int appointmentId)
        {
            var consultation = await _repo.GetByAppointmentIdAsync(appointmentId);
            if (consultation == null) return null;

            return new ConsultationResponse
            {
                Id = consultation.Id,
                AppointmentId = consultation.AppointmentId,
                PatientId = consultation.Appointment?.PatientId ?? 0,
                PatientName = consultation.Appointment?.Patient?.RoleName ?? "",
                DoctorId = consultation.Appointment?.DoctorId ?? 0,
                DoctorName = consultation.Appointment?.Doctor?.FullName ?? "",
                AppointmentDate = consultation.Appointment?.AppointmentDate ?? DateTime.MinValue,
                ConsultationDate = consultation.ConsultationDate,
                Symptoms = consultation.Symptoms ?? string.Empty,
                Diagnosis = consultation.Diagnosis ?? string.Empty,
                Prescription = consultation.Prescription ?? string.Empty,
                TreatmentPlan = consultation.TreatmentPlan ?? string.Empty,
                FollowUpInstructions = consultation.FollowUpInstructions ?? string.Empty,
                FollowUpDate = consultation.FollowUpDate,
                ConsultationFee = consultation.ConsultationFee,
                IsPaid = consultation.IsPaid,
                Notes = consultation.Notes,
                CreatedAt = consultation.CreatedAt
            };
        }

        public async Task<bool> UpdateAsync(int consultationId, ConsultationRequest request)
        {
            var existing = await _repo.GetByIdAsync(consultationId);
            if (existing == null) return false;

            existing.Symptoms = request.Symptoms;
            existing.Diagnosis = request.Diagnosis;
            existing.Prescription = request.Prescription;
            existing.TreatmentPlan = request.TreatmentPlan;
            existing.FollowUpInstructions = request.FollowUpInstructions;
            existing.FollowUpDate = request.FollowUpDate;
            existing.ConsultationFee = request.ConsultationFee;
            existing.Notes = request.Notes;
            existing.UpdatedAt = DateTime.UtcNow;

            return await _repo.UpdateAsync(existing);
        }
    }
}
