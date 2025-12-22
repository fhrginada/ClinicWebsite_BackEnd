namespace PatientApi.Models.ViewModels
{
    public class PatientDashboardResponse
    {
        public string? FirstName { get; set; }
        public List<AppointmentResponse> UpcomingAppointments { get; set; } = new();
        public List<NotificationResponse> Notifications { get; set; } = new();
    }
}
