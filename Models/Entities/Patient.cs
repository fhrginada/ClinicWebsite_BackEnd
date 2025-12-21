using System;
using System.Collections.Generic;

namespace PatientApi.Models.Entities
{
    public class Patient
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        public string? BloodType { get; set; }  

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string? RoleName { get; set; }

        // Navigation Properties
        public User? User { get; set; }

        public ICollection<MedicalHistory> MedicalHistories { get; set; }
            = new List<MedicalHistory>();

        public ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();
    }
}
