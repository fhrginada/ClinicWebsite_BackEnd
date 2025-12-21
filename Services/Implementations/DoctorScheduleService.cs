using PatientApi.Models.Entities;
using PatientApi.Models.ViewModels;
using PatientApi.Repositories.Interfaces;
using PatientApi.Services.Interfaces;

namespace PatientApi.Services.Implementations
{
    public class DoctorScheduleService : IDoctorScheduleService
    {
        private readonly IDoctorScheduleRepository _repo;
        private readonly IDoctorRepository _doctorRepo;
        private readonly IAppointmentRepository _appointmentRepo;

        public DoctorScheduleService(IDoctorScheduleRepository repo, IDoctorRepository doctorRepo, IAppointmentRepository appointmentRepo)
        {
            _repo = repo;
            _doctorRepo = doctorRepo;
            _appointmentRepo = appointmentRepo;
        }

        public async Task<IEnumerable<DoctorSchedule>> GetByDoctorIdAsync(int doctorId)
        {
            return await _repo.GetByDoctorIdAsync(doctorId);
        }

        public async Task AddAsync(DoctorSchedule schedule)
        {
            await _repo.AddAsync(schedule);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<DoctorAvailabilityResponse?> GetDoctorAvailabilityAsync(int doctorId, DateTime? startDate = null, int days = 7)
        {
            var doctor = await _doctorRepo.GetByIdAsync(doctorId);
            if (doctor == null) return null;

            var schedules = (await _repo.GetByDoctorIdAsync(doctorId)).ToList();
            var appointments = (await _appointmentRepo.GetAllAsync())
                .Where(a => a.Status != AppointmentStatus.Cancelled)
                .ToList();

            var start = (startDate ?? DateTime.UtcNow.Date).Date;
            var end = start.AddDays(days);
            var windowSchedules = schedules
                .Where(s => s.Date.Date >= start && s.Date.Date < end)
                .OrderBy(s => s.Date)
                .ThenBy(s => s.StartTime)
                .ToList();

            var availableSlots = new List<AvailableTimeSlot>();

            foreach (var schedule in windowSchedules)
            {
                var slotLabel = $"{schedule.StartTime:hh\\:mm} - {schedule.EndTime:hh\\:mm}";
                var booked = appointments.Any(a => a.DoctorId == doctorId
                                                && a.AppointmentDate.Date == schedule.Date.Date
                                                && a.TimeSlot == slotLabel);

                availableSlots.Add(new AvailableTimeSlot
                {
                    Date = schedule.Date.Date,
                    DayOfWeek = schedule.Date.DayOfWeek.ToString(),
                    StartTime = schedule.StartTime.ToString(@"hh\:mm"),
                    EndTime = schedule.EndTime.ToString(@"hh\:mm"),
                    IsAvailable = !booked
                });
            }

            return new DoctorAvailabilityResponse
            {
                DoctorId = doctor.DoctorId,
                DoctorName = doctor.FullName ?? string.Empty,
                Specialty = doctor.Specialty ?? string.Empty,
                AvailableSlots = availableSlots
            };
        }
    }
}
