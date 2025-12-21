namespace PatientApi.Models.ViewModels
{
    public class PatientRequest
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }
    }
}
