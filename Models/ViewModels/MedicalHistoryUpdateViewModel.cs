using System.ComponentModel.DataAnnotations;

namespace PatientApi.Models.ViewModels;

public class MedicalHistoryUpdateViewModel
{
    [Required]
    public DateTime DateRecorded { get; set; } = DateTime.UtcNow;

    public int? DoctorId { get; set; }

    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }

    public DateTime? FollowUpDate { get; set; }

    [MaxLength(1000)]
    public string? AttachmentUrl { get; set; }
}
