using PatientApi.Models.Entities;

namespace PatientApi.Models.ViewModels;

public static class ViewModelMapper
{
    public static PatientViewModel ToViewModel(Patient p)
    {
        return new PatientViewModel
        {
            Id = p.Id,
            DateOfBirth = p.DateOfBirth,
            Gender = p.Gender.ToString(),
            BloodType = p.BloodType,
            Phone = p.Phone,
            Address = p.Address,
            RoleName = p.RoleName,
            UserId = p.UserId,
            MedicalHistoryCount = p.MedicalHistories?.Count ?? 0
        };
    }

    public static MedicalHistoryViewModel ToViewModel(MedicalHistory m)
    {
        return new MedicalHistoryViewModel
        {
            Id = m.Id,
            PatientId = m.PatientId,
            DoctorId = m.DoctorId,
            DateRecorded = m.DateRecorded,
            FollowUpDate = m.FollowUpDate,
            Diagnosis = m.Diagnosis,
            Treatment = m.Treatment,
            AttachmentUrl = m.AttachmentUrl
        };
    }

}
