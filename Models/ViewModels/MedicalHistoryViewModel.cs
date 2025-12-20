namespace PatientApi.Models.ViewModels{

public class MedicalHistoryViewModel
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int? DoctorId { get; set; }
    public DateTime DateRecorded { get; set; }
    public DateTime? FollowUpDate { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public string? AttachmentUrl { get; set; }
}
}