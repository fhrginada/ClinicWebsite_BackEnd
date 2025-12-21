namespace PatientApi.Models.ViewModels.DoctorVM
{
    public class UpdateDoctorRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
    }
}
