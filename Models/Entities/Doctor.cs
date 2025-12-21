using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PatientApi.Models.Entities
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        [Required, StringLength(150)]
        public string FullName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Specialty { get; set; }

 
        [NotMapped]
        public string Name => FullName;

        [NotMapped]
        public string Specialization => Specialty ?? string.Empty;

        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = null!;

        public string RoleName { get; set; } = "Doctor";

        public ICollection<DoctorSchedule> Schedules { get; set; } = new List<DoctorSchedule>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        
    }
}
