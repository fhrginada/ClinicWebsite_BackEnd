namespace PatientApi.Models.ViewModels.NurseVM
{
    public class NurseResponse
    {
        public int NurseId { get; set; }
        public string FullName { get; set; }
        public string Specialty { get; set; }
        public int? UserId { get; set; }
    }
}
