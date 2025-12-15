namespace PatientApi.DTOs;

public class PatientAttachmentDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string FileName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public long Size { get; set; }
    public string Path { get; set; } = null!;
    public DateTime UploadedAt { get; set; }
}
