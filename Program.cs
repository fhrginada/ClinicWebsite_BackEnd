using Microsoft.EntityFrameworkCore;
using PatientApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories & Services
builder.Services.AddScoped<PatientApi.Repositories.Interfaces.IPatientRepository, PatientApi.Repositories.PatientRepository>();
builder.Services.AddScoped<PatientApi.Repositories.Interfaces.IMedicalHistoryRepository, PatientApi.Repositories.MedicalHistoryRepository>();
builder.Services.AddScoped<PatientApi.Repositories.Interfaces.IAppointmentRepository, PatientApi.Repositories.Implementations.AppointmentRepository>();
builder.Services.AddScoped<PatientApi.Repositories.Interfaces.IConsultationRepository, PatientApi.Repositories.Implementations.ConsultationRepository>();
builder.Services.AddScoped<PatientApi.Repositories.Interfaces.IDoctorRepository, PatientApi.Repositories.Implementation.DoctorRepository>();
builder.Services.AddScoped<PatientApi.Repositories.Interfaces.IDoctorScheduleRepository, PatientApi.Repositories.Implementation.DoctorScheduleRepository>();
builder.Services.AddScoped<PatientApi.Repositories.Interfaces.INurseRepository, PatientApi.Repositories.Implementation.NurseRepository>();
builder.Services.AddScoped<PatientApi.Repositories.Interfaces.INurseScheduleRepository, PatientApi.Repositories.Implementation.NurseScheduleRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

builder.Services.AddScoped<PatientApi.Services.Interfaces.IPatientService, PatientApi.Services.PatientService>();
builder.Services.AddScoped<PatientApi.Services.Interfaces.IMedicalHistoryService, PatientApi.Services.MedicalHistoryService>();
builder.Services.AddScoped<PatientApi.Services.Interfaces.IAppointmentService, PatientApi.Services.Implementations.AppointmentService>();
builder.Services.AddScoped<PatientApi.Services.Interfaces.IConsultationService, PatientApi.Services.Implementations.ConsultationService>();
builder.Services.AddScoped<PatientApi.Services.Interfaces.IDoctorService, PatientApi.Services.Implementations.DoctorService>();
builder.Services.AddScoped<PatientApi.Services.Interfaces.IDoctorScheduleService, PatientApi.Services.Implementations.DoctorScheduleService>();
builder.Services.AddScoped<PatientApi.Services.Interfaces.INurseScheduleService, PatientApi.Services.Implementations.NurseScheduleService>();
builder.Services.AddScoped<PatientApi.Services.Interfaces.IExportService, PatientApi.Services.Implementations.ExportService>();

// ===== Add CORS here =====
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// ===== Use CORS =====
app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
