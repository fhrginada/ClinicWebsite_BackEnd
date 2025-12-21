namespace PatientApi.Models.ViewModels.NurseVM
{
    public class NurseResponse
    {
        public int NurseId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public int? UserId { get; set; }
    }
}
