using System.ComponentModel.DataAnnotations;
using PatientApi.Models;

namespace PatientApi.DTOs;

public class PatientCreateDto
{
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
}
