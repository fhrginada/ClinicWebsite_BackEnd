namespace PatientApi.Models.ViewModels.NurseVM
{
    public class CreateNurseRequest
    {
        public string FullName { get; set; }
        public string Specialty { get; set; }
        public int? UserId { get; set; }
    }
}
