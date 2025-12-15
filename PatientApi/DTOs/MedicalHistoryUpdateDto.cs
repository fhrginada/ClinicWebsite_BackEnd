using System.ComponentModel.DataAnnotations;

namespace PatientApi.DTOs;

public class MedicalHistoryUpdateDto
{
    [Required]
    public DateTime Date { get; set; }

    [Required, MaxLength(1000)]
    public string Diagnosis { get; set; } = null!;

    [MaxLength(2000)]
    public string? Treatment { get; set; }

    [MaxLength(2000)]
    public string? Notes { get; set; }
}
