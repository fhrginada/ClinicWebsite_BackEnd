using System;
using System.ComponentModel.DataAnnotations;

namespace PatientApi.Models.ViewModels
{

    public class ConsultationRequest
    {
        [Required]
        public int? AppointmentId { get; set; } // optional if creating patient and appointment

        [Required]
        [MaxLength(1000)]
        public string Symptoms { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Diagnosis { get; set; }

        [MaxLength(2000)]
        public string Prescription { get; set; }

        [MaxLength(2000)]
        public string TreatmentPlan { get; set; }

        [MaxLength(1000)]
        public string FollowUpInstructions { get; set; }

        public DateTime? FollowUpDate { get; set; }

        [Required]
        [Range(0, 999999)]
        public decimal ConsultationFee { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        public PatientRequest? Patient { get; set; }

    }
}
