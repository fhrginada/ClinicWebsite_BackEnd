using PatientApi.Models.Entities;

namespace PatientApi.Models.ViewModels;

public static class InputMapper
{
    public static Patient ToEntity(PatientCreateViewModel vm)
    {
        return new Patient
        {
            FirstName = vm.FirstName,
            LastName = vm.LastName,
            DateOfBirth = vm.DateOfBirth,
            Gender = vm.Gender,
            Phone = vm.Phone,
            Email = vm.Email
        };
    }

    public static void ApplyUpdate(PatientUpdateViewModel vm, Patient p)
    {
        p.FirstName = vm.FirstName;
        p.LastName = vm.LastName;
        p.DateOfBirth = vm.DateOfBirth;
        p.Gender = vm.Gender;
        p.Phone = vm.Phone;
        p.Email = vm.Email;
    }

    public static MedicalHistory ToEntity(MedicalHistoryCreateViewModel vm)
    {
        return new MedicalHistory
        {
            PatientId = vm.PatientId,
            Date = vm.Date,
            Diagnosis = vm.Diagnosis,
            Treatment = vm.Treatment,
            Notes = vm.Notes
        };
    }

    public static void ApplyUpdate(MedicalHistoryUpdateViewModel vm, MedicalHistory m)
    {
        m.Date = vm.Date;
        m.Diagnosis = vm.Diagnosis;
        m.Treatment = vm.Treatment;
        m.Notes = vm.Notes;
    }
}
