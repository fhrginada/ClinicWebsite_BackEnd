using Microsoft.EntityFrameworkCore;

// Repositories
using PatientApi.Repositories.Interfaces;
using PatientApi.Repositories.Implementation;

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
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IMedicalHistoryRepository, MedicalHistoryRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IConsultationRepository, ConsultationRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IDoctorScheduleRepository, DoctorScheduleRepository>();
builder.Services.AddScoped<INurseRepository, NurseRepository>();
builder.Services.AddScoped<INurseScheduleRepository, NurseScheduleRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
builder.Services.AddScoped<IMedicationRepository, MedicationRepository>();

// ================= Services =================
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IMedicalHistoryService, MedicalHistoryService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IConsultationService, ConsultationService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IDoctorScheduleService, DoctorScheduleService>();
builder.Services.AddScoped<INurseScheduleService, NurseScheduleService>();
builder.Services.AddScoped<IExportService, ExportService>();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
builder.Services.AddScoped<IMedicationService, MedicationService>();
builder.Services.AddScoped<INurseService, NurseService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IPdfGeneratorService, PdfGeneratorService>();
builder.Services.AddScoped<IReportService, ReportService>();

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
