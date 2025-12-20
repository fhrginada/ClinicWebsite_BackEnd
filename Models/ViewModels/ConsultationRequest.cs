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
        public string Symptoms { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string Diagnosis { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string Prescription { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string TreatmentPlan { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string FollowUpInstructions { get; set; } = string.Empty;

        public DateTime? FollowUpDate { get; set; }

        [Required]
        [Range(0, 999999)]
        public decimal ConsultationFee { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; } = string.Empty;

        

    }
}
