using System;

namespace PatientApi.Models.ViewModels
{
    public class AppointmentResponse
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string PatientEmail { get; set; }
        public string PatientPhone { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string DoctorSpecialization { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string TimeSlot { get; set; }
        public string Status { get; set; }  
        public string ReasonForVisit { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool HasConsultation { get; set; }
    }
}