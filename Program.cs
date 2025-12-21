using Microsoft.EntityFrameworkCore;

// Repositories
using PatientApi.Repositories.Interfaces;
using PatientApi.Repositories.Implementations;

// Services
using PatientApi.Services.Interfaces;
using PatientApi.Services.Implementations;

using PatientApi.Data;

var builder = WebApplication.CreateBuilder(args);

// ================= Controllers & Swagger =================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ================= Database =================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ================= Repositories =================
builder.Services.AddScoped<IPatientRepository, PatientApi.Repositories.PatientRepository>();
builder.Services.AddScoped<IMedicalHistoryRepository, PatientApi.Repositories.MedicalHistoryRepository>();
builder.Services.AddScoped<IAppointmentRepository, PatientApi.Repositories.Implementations.AppointmentRepository>();
builder.Services.AddScoped<IConsultationRepository, PatientApi.Repositories.Implementations.ConsultationRepository>();
builder.Services.AddScoped<IDoctorRepository, PatientApi.Repositories.Implementations.DoctorRepository>();
builder.Services.AddScoped<IDoctorScheduleRepository, PatientApi.Repositories.Implementations.DoctorScheduleRepository>();
builder.Services.AddScoped<INurseRepository, PatientApi.Repositories.Implementations.NurseRepository>();
builder.Services.AddScoped<INurseScheduleRepository, PatientApi.Repositories.Implementations.NurseScheduleRepository>();
builder.Services.AddScoped<INotificationRepository, PatientApi.Repositories.Implementations.NotificationRepository>();
builder.Services.AddScoped<IPrescriptionRepository, PatientApi.Repositories.Implementations.PrescriptionRepository>();
builder.Services.AddScoped<IMedicationRepository, PatientApi.Repositories.Implementations.MedicationRepository>();

// ================= Services =================
builder.Services.AddScoped<IPatientService, PatientApi.Services.Implementations.PatientService>();
builder.Services.AddScoped<IMedicalHistoryService, PatientApi.Services.Implementations.MedicalHistoryService>();
builder.Services.AddScoped<IAppointmentService, PatientApi.Services.Implementations.AppointmentService>();
builder.Services.AddScoped<IConsultationService, PatientApi.Services.Implementations.ConsultationService>();
builder.Services.AddScoped<IDoctorService, PatientApi.Services.Implementations.DoctorService>();
builder.Services.AddScoped<IDoctorScheduleService, PatientApi.Services.Implementations.DoctorScheduleService>();
builder.Services.AddScoped<INurseScheduleService, PatientApi.Services.Implementations.NurseScheduleService>();
builder.Services.AddScoped<IExportService, PatientApi.Services.Implementations.ExportService>();
builder.Services.AddScoped<IPrescriptionService, PatientApi.Services.Implementations.PrescriptionService>();
builder.Services.AddScoped<IMedicationService, PatientApi.Services.Implementations.MedicationService>(); builder.Services.AddScoped<INurseService, PatientApi.Services.Implementations.NurseService>();
builder.Services.AddScoped<INotificationService, PatientApi.Services.Implementations.NotificationService>();
builder.Services.AddScoped<IPdfGeneratorService, PatientApi.Services.Implementations.PdfGeneratorService>();
builder.Services.AddScoped<IReportService, PatientApi.Services.Implementations.ReportService>();

// ================= CORS =================
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

// ================= Middleware =================
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
