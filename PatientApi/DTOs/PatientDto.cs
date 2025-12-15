using PatientApi.Models;

namespace PatientApi.DTOs;

public class PatientDto
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public string FirstName { get; set; } = null!;
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<MedicalHistoryDto> MedicalHistories { get; set; } = new();
}
