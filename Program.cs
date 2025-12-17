using Microsoft.EntityFrameworkCore;
using ClinicBackend_Final.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// =========================
// Repositories
// =========================
builder.Services.AddScoped<ClinicBackend_Final.Repositories.Interfaces.IPatientRepository,
                          ClinicBackend_Final.Repositories.PatientRepository>();

builder.Services.AddScoped<ClinicBackend_Final.Repositories.Interfaces.IMedicalHistoryRepository,
                          ClinicBackend_Final.Repositories.MedicalHistoryRepository>();

// 🔴 Prescription System
builder.Services.AddScoped<ClinicBackend_Final.Repositories.Interfaces.IPrescriptionRepository,
                          ClinicBackend_Final.Repositories.PrescriptionRepository>();

builder.Services.AddScoped<ClinicBackend_Final.Repositories.Interfaces.IMedicationRepository,
                          ClinicBackend_Final.Repositories.MedicationRepository>();

// =========================
// Services
// =========================
builder.Services.AddScoped<ClinicBackend_Final.Services.Interfaces.IPatientService,
                          ClinicBackend_Final.Services.PatientService>();

builder.Services.AddScoped<ClinicBackend_Final.Services.Interfaces.IMedicalHistoryService,
                          ClinicBackend_Final.Services.MedicalHistoryService>();

// 🔴 Prescription System
builder.Services.AddScoped<ClinicBackend_Final.Services.Interfaces.IPrescriptionService,
                          ClinicBackend_Final.Services.PrescriptionService>();

builder.Services.AddScoped<ClinicBackend_Final.Services.Interfaces.IMedicationService,
                          ClinicBackend_Final.Services.MedicationService>();

builder.Services.AddScoped<ClinicBackend_Final.Services.Interfaces.IPdfExportService,
                          ClinicBackend_Final.Services.PdfExportService>();

var app = builder.Build();

// Apply pending migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
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
