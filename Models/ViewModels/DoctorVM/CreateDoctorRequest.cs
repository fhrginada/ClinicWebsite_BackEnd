namespace PatientApi.Models.ViewModels.DoctorVM
{
    public class CreateDoctorRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}
