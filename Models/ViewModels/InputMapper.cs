using PatientApi.Models.Entities;

namespace PatientApi.Models.ViewModels;

public static class InputMapper
{
    public static Patient ToEntity(PatientCreateViewModel vm)
    {
        return new Patient
        {
            DateOfBirth = vm.DateOfBirth,
            Gender = vm.Gender,
            BloodType = vm.BloodType,
            Phone = vm.Phone,
            Address = vm.Address,
            RoleName = vm.RoleName,
            UserId = vm.UserId
        };
    }

    public static void ApplyUpdate(PatientUpdateViewModel vm, Patient p)
    {
        p.DateOfBirth = vm.DateOfBirth;
        p.Gender = vm.Gender;
        p.BloodType = vm.BloodType;
        p.Phone = vm.Phone;
        p.Address = vm.Address;
        p.RoleName = vm.RoleName;
        p.UserId = vm.UserId;
    }

    public static MedicalHistory ToEntity(MedicalHistoryCreateViewModel vm)
    {
        return new MedicalHistory
        {
            PatientId = vm.PatientId,
            DoctorId = vm.DoctorId,
            DateRecorded = vm.DateRecorded,
            FollowUpDate = vm.FollowUpDate,
            Diagnosis = vm.Diagnosis,
            Treatment = vm.Treatment,
            AttachmentUrl = vm.AttachmentUrl
        };
    }

    public static void ApplyUpdate(MedicalHistoryUpdateViewModel vm, MedicalHistory m)
    {
        m.DoctorId = vm.DoctorId;
        m.DateRecorded = vm.DateRecorded;
        m.FollowUpDate = vm.FollowUpDate;
        m.Diagnosis = vm.Diagnosis;
        m.Treatment = vm.Treatment;
        m.AttachmentUrl = vm.AttachmentUrl;
    }
}
