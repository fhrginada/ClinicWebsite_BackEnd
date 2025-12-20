namespace PatientApi.Models.ViewModels

{
    public class PatientViewModel
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string? FullName { get; set; }   // ✨ ضيفي ده
        public string? Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? BloodType { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? RoleName { get; set; }
        public int MedicalHistoryCount { get; set; }
    }
}
