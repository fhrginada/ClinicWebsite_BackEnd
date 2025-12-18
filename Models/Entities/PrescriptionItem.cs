namespace ClinicBackend_Final.Models
{
    public class PrescriptionItem
    {
        public int Id { get; set; }
        public int PrescriptionId { get; set; }
        public int MedicationId { get; set; }

        public string Dose { get; set; }
        public string Frequency { get; set; } // e.g., "Once a day"

        public virtual Medication Medication { get; set; }
        public virtual Prescription Prescription { get; set; }
    }
}
