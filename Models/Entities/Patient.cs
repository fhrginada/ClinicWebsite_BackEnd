using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PatientApi.Models.Entities
{
    public class Patient
    {
        public int Id { get; set; }

        // من HEAD
        public string? FullName { get; set; }

        // UserId زي main
        public int? UserId { get; set; }

        // Optional mapping to an external user/doctor system
        public User? User { get; set; }

        public DateTime DateOfBirth { get; set; } // من main

        public Gender Gender { get; set; }

        public string? BloodType { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string? RoleName { get; set; }

        // Navigation Properties
        public ICollection<MedicalHistory> MedicalHistories { get; set; }
            = new List<MedicalHistory>();

        public ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();
    }
}
