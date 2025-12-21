using System;
using System.ComponentModel.DataAnnotations;
using PatientApi.Data;

namespace PatientApi.Models.Entities


{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        public string Message { get; set; }

        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
