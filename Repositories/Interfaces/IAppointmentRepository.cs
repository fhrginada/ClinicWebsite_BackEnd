using PatientApi.Models.Entities;

namespace PatientApi.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<Appointment?> GetByIdAsync(int id);
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId);
        Task<IEnumerable<Appointment>> GetByDoctorIdAsync(int doctorId);
        Task<bool> ExistsConflictAsync(int doctorId, int patientId, DateTime appointmentDate, string timeSlot);
        Task AddAsync(Appointment appointment);
        Task<bool> UpdateAsync(Appointment appointment);
    }
}
