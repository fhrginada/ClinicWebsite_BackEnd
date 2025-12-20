using System;

namespace PatientApi.Models.ViewModels
{
    public class AppointmentResponse
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string PatientEmail { get; set; } = string.Empty;
        public string PatientPhone { get; set; } = string.Empty;
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string DoctorSpecialization { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public string TimeSlot { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;  
        public string ReasonForVisit { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool HasConsultation { get; set; }
    }
}