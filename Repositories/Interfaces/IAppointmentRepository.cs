using PatientApi.Models.Entities;

namespace PatientApi.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<Appointment?> GetByIdAsync(int id);
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task AddAsync(Appointment appointment);
        Task<bool> UpdateAsync(Appointment appointment);
    }
}
