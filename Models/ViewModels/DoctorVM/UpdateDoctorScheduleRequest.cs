namespace PatientApi.Models.ViewModels.DoctorVM
{
    public class UpdateDoctorScheduleRequest
    {
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int SlotDuration { get; set; }
    }
}
