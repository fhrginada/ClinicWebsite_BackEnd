using PatientApi.Models.Entities;

namespace PatientApi.Models.ViewModels
{
    public static class ViewModelMapper
    {
        public static PatientViewModel ToViewModel(Patient p)
        {
            return new PatientViewModel
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                FullName = $"{p.FirstName} {p.LastName}".Trim(),
                Phone = p.Phone,
                Address = p.Address,
                BloodType = p.BloodType,
                Gender = p.Gender.ToString(),
                DateOfBirth = p.DateOfBirth,
                UserId = p.UserId
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
}
