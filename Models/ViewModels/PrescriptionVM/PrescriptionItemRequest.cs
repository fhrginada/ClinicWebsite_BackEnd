namespace PatientApi.Models.ViewModels
{
    public class PrescriptionItemRequest
    {
        public int MedicationId { get; set; }
        public string Dose { get; set; }
        public string Frequency { get; set; }
    }
}
