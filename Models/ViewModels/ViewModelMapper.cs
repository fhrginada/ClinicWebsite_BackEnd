using PatientApi.Models.Entities;

namespace PatientApi.Models.ViewModels;

public static class ViewModelMapper
{
    public static PatientViewModel ToViewModel(Patient p)
    {
        var today = DateTime.UtcNow.Date;
        var age = today.Year - p.DateOfBirth.Year - (p.DateOfBirth.Date > today.AddYears(-(today.Year - p.DateOfBirth.Year)) ? 1 : 0);
        return new PatientViewModel
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
            DateOfBirth = p.DateOfBirth,
            Age = age,
            Gender = p.Gender.ToString(),
            Phone = p.Phone,
            Email = p.Email,
            MedicalHistoryCount = p.MedicalHistories?.Count ?? 0,
            AttachmentCount = p.Attachments?.Count ?? 0
        };
    }

    public static MedicalHistoryViewModel ToViewModel(MedicalHistory m)
    {
        return new MedicalHistoryViewModel
        {
            Id = m.Id,
            PatientId = m.PatientId,
            Date = m.Date,
            Diagnosis = m.Diagnosis,
            Treatment = m.Treatment,
            Notes = m.Notes
        };
    }

    public static PatientAttachmentViewModel ToViewModel(PatientAttachment a, string baseUrl)
    {
        return new PatientAttachmentViewModel
        {
            Id = a.Id,
            PatientId = a.PatientId,
            FileName = a.FileName,
            ContentType = a.ContentType,
            Size = a.Size,
            Url = string.IsNullOrWhiteSpace(baseUrl) ? a.Path : (baseUrl.TrimEnd('/') + "/" + a.Path.TrimStart('/')), 
            UploadedAt = a.UploadedAt
        };
    }
}
