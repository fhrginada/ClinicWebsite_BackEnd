namespace ClinicalBackend.Models
{
    public class PrescriptionDetail
    {
        public int PrescriptionDetailID { get; set; }

        public int PrescriptionID { get; set; }

        public string DrugName { get; set; } = string.Empty;
        public string? Dosage { get; set; }
        public string? Frequency { get; set; }
        public string? Duration { get; set; }

        // Navigation property للرجوع للـ Prescription
        public Prescription? Prescription { get; set; }
    }
}
