namespace PatientApi.Models.ViewModels.DoctorVM
{
    public class DoctorResponse
    {
        public int DoctorId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}
