namespace PatientApi.Models.Entities
{
    public class PrescriptionDetail
    {
        public int PrescriptionDetailId { get; set; }
        public int PrescriptionId { get; set; }
        public int MedicationId { get; set; }

        public string Dose { get; set; }
        public string Frequency { get; set; }

        public Prescription Prescription { get; set; }
        public Medication Medication { get; set; }
    }
}
