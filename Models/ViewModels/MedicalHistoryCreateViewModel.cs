using System.ComponentModel.DataAnnotations;

namespace PatientApi.Models.ViewModels;

public class MedicalHistoryCreateViewModel
{
    [Required]
    public int PatientId { get; set; }

    [Required]
    public DateTime Date { get; set; } = DateTime.UtcNow;

    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public string? Notes { get; set; }
}
