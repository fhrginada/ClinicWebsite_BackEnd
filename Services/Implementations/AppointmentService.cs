using PatientApi.Models.Entities;
using PatientApi.Models.ViewModels;
using PatientApi.Repositories.Interfaces;
using PatientApi.Services.Interfaces;

namespace PatientApi.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repo;

        public AppointmentService(IAppointmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<int> CreateAsync(AppointmentRequest request)
        {
            var appointment = new Appointment
            {
                PatientId = request.PatientId,
                DoctorId = request.DoctorId,
                AppointmentDate = request.AppointmentDate,
                TimeSlot = request.TimeSlot,
                ReasonForVisit = request.ReasonForVisit ?? "",
                CreatedBy = "System"
            };

            await _repo.AddAsync(appointment);
            return appointment.AppointmentId;
        }

        public async Task<bool> UpdateStatusAsync(AppointmentStatusRequest request)
        {
            var appointment = await _repo.GetByIdAsync(request.AppointmentId);
            if (appointment == null) return false;

            appointment.Status = Enum.Parse<AppointmentStatus>(request.Status, true);
            appointment.UpdatedAt = DateTime.UtcNow;

            return await _repo.UpdateAsync(appointment);
        }

        public async Task<IEnumerable<AppointmentResponse>> GetAllAsync()
        {
            var appointments = await _repo.GetAllAsync();

            return appointments.Select(a => new AppointmentResponse
            {
                Id = a.AppointmentId,
                DoctorId = a.DoctorId,
                PatientId = a.PatientId,
                DoctorName = a.Doctor?.FullName ?? string.Empty,
                DoctorSpecialization = a.Doctor?.Specialty ?? string.Empty,
                PatientName = a.Patient?.User?.UserName ?? string.Empty,
                PatientEmail = a.Patient?.User?.Email ?? string.Empty,
                PatientPhone = a.Patient?.Phone ?? string.Empty,
                AppointmentDate = a.AppointmentDate,
                TimeSlot = a.TimeSlot,
                Status = a.Status.ToString(),
                ReasonForVisit = a.ReasonForVisit ?? string.Empty,
                Notes = a.Notes ?? string.Empty,
                CreatedAt = a.CreatedAt,
                HasConsultation = a.Consultation != null
            });
        }
    }
}
