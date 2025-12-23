using PatientApi.Models.Entities;
using PatientApi.Models.ViewModels;
using PatientApi.Repositories.Interfaces;
using PatientApi.Services.Interfaces;

namespace PatientApi.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repo;
        private readonly IDoctorScheduleRepository _doctorScheduleRepo;

        public AppointmentService(IAppointmentRepository repo, IDoctorScheduleRepository doctorScheduleRepo)
        {
            _repo = repo;
            _doctorScheduleRepo = doctorScheduleRepo;
        }

        public async Task<int> CreateAsync(int patientId, AppointmentRequest request)
        {
            if (request.PatientId.HasValue)
            {
                throw new ArgumentException("PatientId must not be provided.");
            }

            if (request.AppointmentDate == default)
            {
                throw new ArgumentException("AppointmentDate is required.");
            }

            if (string.IsNullOrWhiteSpace(request.TimeSlot))
            {
                throw new ArgumentException("TimeSlot is required.");
            }

            var (start, end) = ParseTimeSlot(request.TimeSlot);
            var scheduleExists = (await _doctorScheduleRepo.GetByDoctorIdAsync(request.DoctorId))
                .Any(s => s.Date.Date == request.AppointmentDate.Date
                       && s.StartTime == start
                       && s.EndTime == end);

            if (!scheduleExists)
            {
                throw new ArgumentException("Selected time slot is not available.");
            }

            var hasConflict = await _repo.ExistsConflictAsync(request.DoctorId, patientId, request.AppointmentDate, request.TimeSlot);
            if (hasConflict)
            {
                throw new ArgumentException("Time conflict. Please select another slot.");
            }

            var appointment = new Appointment
            {
                PatientId = patientId,
                DoctorId = request.DoctorId,
                AppointmentDate = request.AppointmentDate,
                TimeSlot = request.TimeSlot,
                ReasonForVisit = request.ReasonForVisit ?? "",
                CreatedBy = "System"
            };

            await _repo.AddAsync(appointment);
            return appointment.AppointmentId;
        }

        public async Task<bool> UpdateStatusAsync(AppointmentStatusRequest request, int? actingDoctorId = null)
        {
            var appointment = await _repo.GetByIdAsync(request.AppointmentId);
            if (appointment == null) return false;

            if (actingDoctorId.HasValue && appointment.DoctorId != actingDoctorId.Value)
            {
                return false;
            }

            appointment.Status = Enum.Parse<AppointmentStatus>(request.Status, true);
            appointment.UpdatedAt = DateTime.UtcNow;

            return await _repo.UpdateAsync(appointment);
        }

        public async Task<IEnumerable<AppointmentResponse>> GetAllAsync()
        {
            return Map(await _repo.GetAllAsync());
        }

        public async Task<IEnumerable<AppointmentResponse>> GetForPatientAsync(int patientId)
        {
            return Map(await _repo.GetByPatientIdAsync(patientId));
        }

        public async Task<IEnumerable<AppointmentResponse>> GetForDoctorAsync(int doctorId)
        {
            return Map(await _repo.GetByDoctorIdAsync(doctorId));
        }

        private static IEnumerable<AppointmentResponse> Map(IEnumerable<Appointment> appointments)
        {
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

        private static (TimeSpan start, TimeSpan end) ParseTimeSlot(string timeSlot)
        {
            var parts = timeSlot.Split('-', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
            {
                throw new ArgumentException("Invalid TimeSlot format. Expected 'HH:mm - HH:mm'.");
            }

            if (!TimeSpan.TryParse(parts[0], out var start) || !TimeSpan.TryParse(parts[1], out var end))
            {
                throw new ArgumentException("Invalid TimeSlot format. Expected 'HH:mm - HH:mm'.");
            }

            return (start, end);
        }
    }
}
