using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientApi.Models.Entities
{
    public class Consultation
    {
        [Key]
        public int Id { get; set; }

        public int AppointmentId { get; set; }

        [ForeignKey("AppointmentId")]
        public Appointment? Appointment { get; set; }  

        public DateTime ConsultationDate { get; set; } = DateTime.UtcNow;

        [MaxLength(1000)]
        public string? Symptoms { get; set; }

        [MaxLength(1000)]
        public string? Diagnosis { get; set; }

        [MaxLength(2000)]
        public string? Prescription { get; set; }

        [MaxLength(2000)]
        public string? TreatmentPlan { get; set; }

        [MaxLength(1000)]
        public string? FollowUpInstructions { get; set; }

        public DateTime? FollowUpDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ConsultationFee { get; set; }

        public bool IsPaid { get; set; } = false;

        [MaxLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        [MaxLength(100)]
        public string? CreatedBy { get; set; }

        [MaxLength(100)]
        public string? UpdatedBy { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
