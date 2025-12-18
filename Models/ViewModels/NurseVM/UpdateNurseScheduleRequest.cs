namespace PatientApi.Models.ViewModels.NurseVM
{
    public class UpdateNurseScheduleRequest
    {
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
