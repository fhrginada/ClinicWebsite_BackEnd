namespace PatientApi.Models.Entities
{
    public class Medication
    {
        public int MedicationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<PrescriptionDetail> PrescriptionDetails { get; set; }
    }
}
