using System;
using System.ComponentModel.DataAnnotations;
using ClinicalProject_API.Data;

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

        
        [Key] 
        public int ClinicSettingsId { get; set; }

        [Required, StringLength(100)]
        public required string Key { get; set; }

        [StringLength(1024)]
        public required string Value { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
