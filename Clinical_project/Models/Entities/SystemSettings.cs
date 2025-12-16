using System.ComponentModel.DataAnnotations;

namespace Clinical_project.Models.Entities
{
    public class SystemSettings
    {
       
        [Key]
        public int Id { get; set; } = 1;

        [Required]
        public string PlatformName { get; set; } = "Clinical Project System";

        [Required]
        public string TimeZoneId { get; set; } = "Egypt Standard Time";

        public int DefaultConsultationDurationMinutes { get; set; } = 30;

        public string SupportEmail { get; set; } = "support@clinical.com";
    }
}
