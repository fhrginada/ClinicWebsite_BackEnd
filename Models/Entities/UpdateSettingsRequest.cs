namespace Clinical_project.Models.Entities
{
    public class UpdateSettingsRequest
    {
        public string PlatformName { get; set; } = null!;
        public string TimeZoneId { get; set; } = null!;
        public int DefaultConsultationDurationMinutes { get; set; }
        public string SupportEmail { get; set; } = null!;
    }
}