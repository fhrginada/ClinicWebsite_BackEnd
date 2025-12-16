namespace PatientApi.Models.ViewModels.DoctorVM
{
    public class DoctorResponse
    {
        public int DoctorId { get; set; }
        public string FullName { get; set; }
        public string Specialty { get; set; }
        public int UserId { get; set; }
    }
}
