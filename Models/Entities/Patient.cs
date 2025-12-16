using System.ComponentModel.DataAnnotations;

namespace PatientApi.Models.Entities;

public enum Gender { Unknown = 0, Male = 1, Female = 2, Other = 3 }

public class Patient
{
    public int Id { get; set; }

    // Optional mapping to an external user/doctor system
    [MaxLength(100)]
    public string? UserId { get; set; }

    public DateTime DateOfBirth { get; set; }

    public Gender Gender { get; set; } = Gender.Unknown;

    [MaxLength(3)]
    public string? BloodType { get; set; }

    [MaxLength(50)]
    public string? Phone { get; set; }

    [MaxLength(300)]
    public string? Address { get; set; }

    [MaxLength(50)]
    public string? RoleName { get; set; }

    public ICollection<MedicalHistory> MedicalHistories { get; set; } = new List<MedicalHistory>();
}
