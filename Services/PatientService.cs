using Microsoft.EntityFrameworkCore;
using PatientApi.Models.Entities;
using PatientApi.Repositories.Interfaces;
using PatientApi.Services.Interfaces;
using PatientApi.Models.ViewModels;

namespace PatientApi.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _repo;
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly INotificationRepository _notificationRepo;

    public PatientService(IPatientRepository repo, IAppointmentRepository appointmentRepo, INotificationRepository notificationRepo)
    {
        _repo = repo;
        _appointmentRepo = appointmentRepo;
        _notificationRepo = notificationRepo;
    }

    private static readonly HashSet<string> _validBloodTypes = new(new[] 
    { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" }, StringComparer.OrdinalIgnoreCase);

    public async Task<(int total, List<PatientViewModel> items)> GetAsync(int? gender, int page, int pageSize)
    {
        var q = _repo.Query();
        if (gender.HasValue) q = q.Where(p => (int)p.Gender == gender.Value);

        var total = await q.CountAsync();
        var list = await q.OrderBy(p => p.Id)
                          .Skip((page - 1) * pageSize)
                          .Take(pageSize)
                          .Include(p => p.MedicalHistories)
                          .ToListAsync();

        foreach (var p in list)
            foreach (var m in p.MedicalHistories) m.Patient = null;

        var vm = list.Select(ViewModelMapper.ToViewModel).ToList();
        return (total, vm);
    }

    public async Task<PatientViewModel?> GetByIdAsync(int id)
    {
        var p = await _repo.Query()
                           .Include(x => x.MedicalHistories)
                           .FirstOrDefaultAsync(x => x.Id == id);
        if (p == null) return null;

        foreach (var m in p.MedicalHistories) m.Patient = null;
        return ViewModelMapper.ToViewModel(p);
    }

    public async Task<PatientViewModel> CreateAsync(Patient dto)
    {
        var today = DateTime.UtcNow.Date;
        if (dto.DateOfBirth >= today) throw new ArgumentException("DateOfBirth must be in the past");
        var age = today.Year - dto.DateOfBirth.Year - (dto.DateOfBirth.Date > today.AddYears(-(today.Year - dto.DateOfBirth.Year)) ? 1 : 0);
        if (age <= 0 || age > 150) throw new ArgumentException("Invalid age");
        if (dto.Gender == Gender.Unknown) throw new ArgumentException("Gender must be Male, Female or Other");
        var normalizedBloodType = NormalizeBloodType(dto.BloodType);

        var p = new Patient
        {
            UserId = dto.UserId,
            DateOfBirth = dto.DateOfBirth,
            Gender = dto.Gender,
            BloodType = normalizedBloodType,
            Phone = dto.Phone,
            Address = dto.Address,
            RoleName = dto.RoleName,
        };
        await _repo.AddAsync(p);
        await _repo.SaveChangesAsync();

        p.MedicalHistories = new List<MedicalHistory>();
        return ViewModelMapper.ToViewModel(p);
    }

    public async Task<bool> UpdateAsync(int id, Patient dto)
    {
        var p = await _repo.FindAsync(id);
        if (p == null) return false;

        var today = DateTime.UtcNow.Date;
        if (dto.DateOfBirth >= today) throw new ArgumentException("DateOfBirth must be in the past");
        var age = today.Year - dto.DateOfBirth.Year - (dto.DateOfBirth.Date > today.AddYears(-(today.Year - dto.DateOfBirth.Year)) ? 1 : 0);
        if (age <= 0 || age > 150) throw new ArgumentException("Invalid age");
        if (dto.Gender == Gender.Unknown) throw new ArgumentException("Gender must be Male, Female or Other");
        var normalizedBloodType = NormalizeBloodType(dto.BloodType);

        p.DateOfBirth = dto.DateOfBirth;
        p.Gender = dto.Gender;
        p.BloodType = normalizedBloodType;
        p.Phone = dto.Phone;
        p.Address = dto.Address;
        p.RoleName = dto.RoleName;
        p.UserId = dto.UserId;

        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var p = await _repo.FindAsync(id);
        if (p == null) return false;

        _repo.Remove(p);
        await _repo.SaveChangesAsync();
        return true;
    }

    private static string? NormalizeBloodType(string? bloodType)
    {
        if (string.IsNullOrWhiteSpace(bloodType)) return null;
        var normalized = bloodType.Trim().ToUpperInvariant();
        if (!_validBloodTypes.Contains(normalized)) throw new ArgumentException("Invalid blood type");
        return normalized;
    }

    public async Task<PatientDashboardResponse?> GetDashboardAsync(int patientId)
    {
        var patient = await GetByIdAsync(patientId);
        if (patient == null) return null;

        var allAppointments = await _appointmentRepo.GetAllAsync();
        var upcomingAppointments = allAppointments
            .Where(a => a.PatientId == patientId && a.AppointmentDate >= DateTime.UtcNow)
            .OrderBy(a => a.AppointmentDate)
            .Take(5)
            .Select(a => new AppointmentResponse
            {
                Id = a.AppointmentId,
                PatientId = a.PatientId,
                PatientName = a.Patient?.RoleName ?? string.Empty,
                PatientEmail = a.Patient?.User?.Email ?? string.Empty,
                PatientPhone = a.Patient?.Phone ?? string.Empty,
                DoctorId = a.DoctorId,
                DoctorName = a.Doctor?.FullName ?? string.Empty,
                DoctorSpecialization = a.Doctor?.Specialty ?? string.Empty,
                AppointmentDate = a.AppointmentDate,
                TimeSlot = a.TimeSlot,
                Status = a.Status.ToString(),
                ReasonForVisit = a.ReasonForVisit,
                Notes = a.Notes ?? string.Empty,
                CreatedAt = a.CreatedAt,
                HasConsultation = a.Consultation != null
            })
            .ToList();

        var notificationResponses = new List<NotificationResponse>();
        if (patient.UserId.HasValue)
        {
            var notifications = await _notificationRepo.GetByUserIdAsync(patient.UserId.Value);
            notificationResponses = notifications
                .OrderByDescending(n => n.CreatedAt)
                .Take(10)
                .Select(n => new NotificationResponse
                {
                    Id = n.NotificationId,
                    Title = n.Title,
                    Message = n.Message,
                    IsRead = n.IsRead,
                    CreatedAt = n.CreatedAt
                })
                .ToList();
        }

        return new PatientDashboardResponse
        {
            PatientInfo = patient,
            UpcomingAppointments = upcomingAppointments,
            Notifications = notificationResponses
        };
    }
}
