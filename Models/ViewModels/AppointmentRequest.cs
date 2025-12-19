using System;
using System.ComponentModel.DataAnnotations;

namespace PatientApi.Models.ViewModels
{
    public class AppointmentRequest
    {
        [Required]
        public int PatientId { get; set; }  // existing patient ID

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string TimeSlot { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? ReasonForVisit { get; set; }
    }
}
