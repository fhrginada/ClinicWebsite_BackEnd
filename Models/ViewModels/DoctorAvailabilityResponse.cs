namespace PatientApi.Models.ViewModels
{
    public class DoctorAvailabilityResponse
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public List<AvailableTimeSlot> AvailableSlots { get; set; } = new();
    }

    public class AvailableTimeSlot
    {
        public DateTime Date { get; set; }
        public string DayOfWeek { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
    }
}
