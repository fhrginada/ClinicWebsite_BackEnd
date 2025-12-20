using Microsoft.EntityFrameworkCore;
using PatientApi.Data;
using PatientApi.Models.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped<PatientApi.Repositories.Interfaces.IPatientRepository, PatientApi.Repositories.PatientRepository>();
builder.Services.AddScoped<PatientApi.Repositories.Interfaces.IMedicalHistoryRepository, PatientApi.Repositories.MedicalHistoryRepository>();
builder.Services.AddScoped<PatientApi.Repositories.Interfaces.IAppointmentRepository, PatientApi.Repositories.Implementations.AppointmentRepository>();
builder.Services.AddScoped<PatientApi.Repositories.Interfaces.IConsultationRepository, PatientApi.Repositories.Implementations.ConsultationRepository>();
builder.Services.AddScoped<PatientApi.Repositories.Interfaces.IDoctorRepository, PatientApi.Repositories.Implementation.DoctorRepository>();
builder.Services.AddScoped<PatientApi.Repositories.Interfaces.IDoctorScheduleRepository, PatientApi.Repositories.Implementation.DoctorScheduleRepository>();
builder.Services.AddScoped<PatientApi.Repositories.Interfaces.INurseRepository, PatientApi.Repositories.Implementation.NurseRepository>();
builder.Services.AddScoped<PatientApi.Repositories.Interfaces.INurseScheduleRepository, PatientApi.Repositories.Implementation.NurseScheduleRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

// Register services
builder.Services.AddScoped<PatientApi.Services.Interfaces.IPatientService, PatientApi.Services.PatientService>();
builder.Services.AddScoped<PatientApi.Services.Interfaces.IMedicalHistoryService, PatientApi.Services.MedicalHistoryService>();
builder.Services.AddScoped<PatientApi.Services.Interfaces.IAppointmentService, PatientApi.Services.Implementations.AppointmentService>();
builder.Services.AddScoped<PatientApi.Services.Interfaces.IConsultationService, PatientApi.Services.Implementations.ConsultationService>();
builder.Services.AddScoped<PatientApi.Services.Interfaces.IDoctorService, PatientApi.Services.Implementations.DoctorService>();
builder.Services.AddScoped<PatientApi.Services.Interfaces.IDoctorScheduleService, PatientApi.Services.Implementations.DoctorScheduleService>();
builder.Services.AddScoped<PatientApi.Services.Interfaces.INurseScheduleService, PatientApi.Services.Implementations.NurseScheduleService>();
builder.Services.AddScoped<PatientApi.Services.Interfaces.IExportService, PatientApi.Services.Implementations.ExportService>();

var app = builder.Build();

// Apply pending EF Core migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    // Seed sample data for quick testing if database is empty
    if (!db.Patients.Any())
    {
        var patients = new[]
        {
            new Patient
            {
                DateOfBirth = DateTime.UtcNow.AddYears(-25),
                Gender = Gender.Male,
                BloodType = "O+",
                Phone = "0123456789",
                Address = "Cairo, Egypt",
                RoleName = "Patient"
            },
            new Patient
            {
                DateOfBirth = DateTime.UtcNow.AddYears(-30),
                Gender = Gender.Female,
                BloodType = "A+",
                Phone = "0987654321",
                Address = "Alexandria, Egypt",
                RoleName = "Patient"
            },
            new Patient
            {
                DateOfBirth = DateTime.UtcNow.AddYears(-35),
                Gender = Gender.Male,
                BloodType = "B+",
                Phone = "0501234567",
                Address = "Giza, Egypt",
                RoleName = "Patient"
            },
            new Patient
            {
                DateOfBirth = DateTime.UtcNow.AddYears(-28),
                Gender = Gender.Female,
                BloodType = "AB+",
                Phone = "0556789012",
                Address = "Helwan, Egypt",
                RoleName = "Patient"
            },
            new Patient
            {
                DateOfBirth = DateTime.UtcNow.AddYears(-45),
                Gender = Gender.Male,
                BloodType = "O-",
                Phone = "0611223344",
                Address = "Zamalek, Cairo",
                RoleName = "Patient"
            }
        };

        foreach (var patient in patients)
        {
            db.Patients.Add(patient);
        }
        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
