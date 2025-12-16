namespace PatientApi.Models.Entities;

public class MedicalHistory
{
    public int Id { get; set; }

    public int PatientId { get; set; }
    public Patient? Patient { get; set; }

    public int? DoctorId { get; set; }

    public DateTime DateRecorded { get; set; } = DateTime.UtcNow;

    public DateTime? FollowUpDate { get; set; }

    public string? Diagnosis { get; set; }

    public string? Treatment { get; set; }

    public string? AttachmentUrl { get; set; }
}
