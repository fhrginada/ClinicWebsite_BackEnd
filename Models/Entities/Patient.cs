using System.ComponentModel.DataAnnotations;

namespace PatientApi.Models.Entities;

public enum Gender { Unknown = 0, Male = 1, Female = 2, Other = 3 }

public class Patient
{
    public int Id { get; set; }

    // Optional mapping to an external user/doctor system
    public string? UserId { get; set; }

    [Required, MaxLength(100)]
    public string FirstName { get; set; } = null!;

    [MaxLength(100)]
    public string? LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public Gender Gender { get; set; } = Gender.Unknown;

    [MaxLength(50)]
    public string? Phone { get; set; }

    [EmailAddress, MaxLength(200)]
    public string? Email { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<MedicalHistory> MedicalHistories { get; set; } = new List<MedicalHistory>();
    public ICollection<PatientAttachment> Attachments { get; set; } = new List<PatientAttachment>();
}
