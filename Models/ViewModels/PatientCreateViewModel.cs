using System;
using System.ComponentModel.DataAnnotations;    
using PatientApi.Models.Entities;




namespace PatientApi.Models.ViewModels{

public class PatientCreateViewModel
{
    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public Gender Gender { get; set; } = Gender.Unknown;

    [MaxLength(3)]
    [RegularExpression("^(A|B|AB|O)[+-]$", ErrorMessage = "Blood type must be A+, A-, B+, B-, AB+, AB-, O+, or O-")]
    public string? BloodType { get; set; }

    [MaxLength(50)]
    public string? Phone { get; set; }

    [MaxLength(300)]
    public string? Address { get; set; }

    [MaxLength(50)]
    public string? RoleName { get; set; }

    public int? UserId { get; set; }
}
}