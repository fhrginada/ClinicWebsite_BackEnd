using System;
using System.ComponentModel.DataAnnotations;
using PatientApi.Data;

namespace PatientApi.Models.Entities
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

    
        
        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }

        public int DoctorId { get; set; }
        public int PatientId { get; set; }

        public DateTime AppointmentDate { get; set; }

        [MaxLength(50)]
        public required string TimeSlot { get; set; }

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;

        [MaxLength(500)]
        public required string ReasonForVisit { get; set; }  

        [MaxLength(1000)]
        public string? Notes { get; set; }  

        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public required string CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [MaxLength(100)]
        public string? UpdatedBy { get; set; }

        public bool IsDeleted { get; set; } = false;

        public Consultation? Consultation { get; set; }

        public int Id => AppointmentId;
    }

    //public enum AppointmentStatus
    //{
    //    Scheduled,
    //    Confirmed,
    //    Completed,
    //    Cancelled,
    //    NoShow
    //}
}




