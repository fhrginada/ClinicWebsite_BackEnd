using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Clinical_project.Models;


namespace Clinical_project.Models.Entities
{
    public class Patient
    {
        [Key]
        public int PatientID { get; set; }

        public required string UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; } = null!;

      
        public string Username { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string BloodType { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string RoleName { get; set; } = "Patient";

       
        
    }
}