using System;

namespace   PatientApi.Models.ViewModels
{
    public class ConsultationResponse
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public DateTime ConsultationDate { get; set; }
        public required string Symptoms { get; set; }
        public required string Diagnosis { get; set; }
        public required string Prescription { get; set; }
        public required string TreatmentPlan { get; set; }
        public required string FollowUpInstructions { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public decimal ConsultationFee { get; set; }
        public bool IsPaid { get; set; }
        public string? Notes { get; set; }  
        public DateTime CreatedAt { get; set; }
        public required string PatientName { get; set; }
        public required string DoctorName { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}