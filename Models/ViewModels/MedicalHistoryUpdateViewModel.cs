using System.ComponentModel.DataAnnotations;

namespace PatientApi.Models.ViewModels;

public class MedicalHistoryUpdateViewModel
{
    [Required]
    public DateTime Date { get; set; } = DateTime.UtcNow;

    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public string? Notes { get; set; }
}
