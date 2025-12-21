using System;
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

        // هذه الحقول كانت تسبب مشكلة Required
        [StringLength(100)]
        public string Key { get; set; } = string.Empty;

        [StringLength(1024)]
        public string Value { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
