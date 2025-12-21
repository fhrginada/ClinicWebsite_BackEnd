namespace PatientApi.Models.ViewModels
{
    public class PatientDashboardResponse
    {
        public List<AppointmentResponse> UpcomingAppointments { get; set; } = new();
        public List<NotificationResponse> Notifications { get; set; } = new();
        public PatientViewModel? PatientInfo { get; set; }
    }
}
