using System.ComponentModel.DataAnnotations;

namespace ClinicalBackend.Models
{
    public class Prescription
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string PatientName { get; set; } = string.Empty;

        [Required]
        public string MedicationName { get; set; } = string.Empty;

        public string Details { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiryDate { get; set; }

        // Navigation property لربط Prescription بالـ Details
        public ICollection<PrescriptionDetail>? PrescriptionDetails { get; set; }
    }
}
