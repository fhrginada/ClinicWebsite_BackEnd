using System.ComponentModel.DataAnnotations;
using PatientApi.Models.Entities;

namespace PatientApi.Models.ViewModels;

public class PatientUpdateViewModel
{
    [Required, MaxLength(100)]
    public string FirstName { get; set; } = null!;

    [MaxLength(100)]
    public string? LastName { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public Gender Gender { get; set; } = Gender.Unknown;

    [MaxLength(50)]
    public string? Phone { get; set; }

    [EmailAddress, MaxLength(200)]
    public string? Email { get; set; }
}
