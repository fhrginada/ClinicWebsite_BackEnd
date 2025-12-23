using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PatientApi.Models.Entities
{

    public class User : IdentityUser<int>
    {


        public string FullName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryDate { get; set; }

        public string? Address { get; set; }

        public UserRole Role { get; set; } = UserRole.Patient;

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public Doctor? DoctorProfile { get; set; }
        public Nurse? NurseProfile { get; set; }
        public Patient? PatientProfile { get; set; }

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
