namespace PatientApi.ViewModels.PrescriptionViewModels
{
    public class MedicationViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Strength { get; set; } = string.Empty;
    }
}
