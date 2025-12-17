namespace PatientApi.ViewModels.PrescriptionViewModels
{
    public class PrescriptionItemViewModel
    {
        public int MedicationId { get; set; }
        public string Dose { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
    }
}
