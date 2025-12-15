namespace PatientApi.Models.ViewModels;

public class PatientAttachmentViewModel
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string FileName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public long Size { get; set; }
    public string Url { get; set; } = null!;
    public DateTime UploadedAt { get; set; }
}