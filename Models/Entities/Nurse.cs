using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PatientApi.Data;

namespace PatientApi.Models.Entities


{
    public class Nurse
    {
        [Key]
        public int NurseId { get; set; }

        [Required, StringLength(150)]
        public string FullName { get; set; } = string.Empty;

        [StringLength(100)]
        public string Specialty { get; set; } = string.Empty;

        public int? UserId { get; set; } 
        public User? User { get; set; }

        public string RoleName { get; set; } = "Nurse";

        public ICollection<NurseSchedule> Schedules { get; set; } = new List<NurseSchedule>();
    }

}
