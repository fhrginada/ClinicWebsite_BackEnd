namespace PatientApi.Models.ViewModels;

public class PatientViewModel
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; } = "Unknown";
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public int MedicalHistoryCount { get; set; }
    public int AttachmentCount { get; set; }
}