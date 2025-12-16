namespace   PatientApi.Models.ViewModels.NurseVM
{
    public class CreateNurseScheduleRequest
    {
        public int NurseId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
