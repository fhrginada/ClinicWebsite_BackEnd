using System;
using System.ComponentModel.DataAnnotations;


namespace PatientApi.Models.Entities

{
    public class SystemSettings
    {
        
        [Key] 
        public int ClinicSettingsId { get; set; }

        [Required, StringLength(100)]
        public required string Key { get; set; }

        [StringLength(1024)]
        public required string Value { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
