using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PatientApi.Data;

namespace PatientApi.Models.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required, StringLength(100)]
        public string UserName { get; set; }

        [Required, StringLength(150)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

       
        public UserRole Role { get; set; } = UserRole.Patient;

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        
        public Doctor DoctorProfile { get; set; }
        public Nurse NurseProfile { get; set; }
        public Patient PatientProfile { get; set; }

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
    }

    
    public enum UserRole
    {
        Admin,
        Doctor,
        Nurse,
        Patient
    }
}
