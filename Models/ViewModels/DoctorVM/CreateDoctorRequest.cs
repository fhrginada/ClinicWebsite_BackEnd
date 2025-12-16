namespace PatientApi.Models.ViewModels.DoctorVM
{
    public class CreateDoctorRequest
    {
        public string FullName { get; set; }
        public string Specialty { get; set; }
        public int UserId { get; set; }
    }
}
