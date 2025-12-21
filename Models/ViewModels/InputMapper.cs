using PatientApi.Models.Entities;
using PatientApi.Models.ViewModels;

namespace PatientApi.Models.ViewModels
{
    public static class InputMapper
    {
        public static Patient ToEntity(PatientCreateViewModel request)
        {
            return new Patient
            {
                Phone = request.Phone,
                Address = request.Address,
                BloodType = request.BloodType,
                Gender = request.Gender,
                DateOfBirth = request.DateOfBirth
            };
        }

        public static void ApplyUpdate(Patient patient, PatientUpdateViewModel request)
        {
            patient.Phone = request.Phone;
            patient.Address = request.Address;
            patient.BloodType = request.BloodType;
            patient.Gender = request.Gender;
            patient.DateOfBirth = request.DateOfBirth;
        }

        public static MedicalHistory ToEntity(MedicalHistoryCreateViewModel request)
        {
            return new MedicalHistory
            {
                PatientId = request.PatientId,
                DateRecorded = request.DateRecorded,
                DoctorId = request.DoctorId,
                Diagnosis = request.Diagnosis,
                Treatment = request.Treatment,
                FollowUpDate = request.FollowUpDate,
                AttachmentUrl = request.AttachmentUrl
            };
        }

        public static void ApplyUpdate(MedicalHistory medicalHistory, MedicalHistoryUpdateViewModel request)
        {
            medicalHistory.DateRecorded = request.DateRecorded;
            medicalHistory.DoctorId = request.DoctorId;
            medicalHistory.Diagnosis = request.Diagnosis;
            medicalHistory.Treatment = request.Treatment;
            medicalHistory.FollowUpDate = request.FollowUpDate;
            medicalHistory.AttachmentUrl = request.AttachmentUrl;
        }
    }
}
