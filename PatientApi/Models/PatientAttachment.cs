namespace PatientApi.Models;

public class PatientAttachment
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public Patient? Patient { get; set; }

    public string FileName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public long Size { get; set; }
    public string Path { get; set; } = null!; // relative path on disk
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}
