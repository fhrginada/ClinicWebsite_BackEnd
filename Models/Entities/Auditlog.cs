using System;
using System.ComponentModel.DataAnnotations;
using   PatientApi.Data;

namespace PatientApi.Models.Entities


{
    public class AuditLog
    {
        [Key]
        public int AuditLogId { get; set; }
        public int? UserId { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public required string Details { get; set; }
    }
}
