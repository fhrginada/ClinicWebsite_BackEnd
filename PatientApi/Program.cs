using Microsoft.EntityFrameworkCore;
using PatientApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services
// repositories
builder.Services.AddScoped<PatientApi.Services.Repositories.Interfaces.IPatientRepository, PatientApi.Services.Repositories.PatientRepository>();
builder.Services.AddScoped<PatientApi.Services.Repositories.Interfaces.IMedicalHistoryRepository, PatientApi.Services.Repositories.MedicalHistoryRepository>();
builder.Services.AddScoped<PatientApi.Services.Repositories.Interfaces.IPatientAttachmentRepository, PatientApi.Services.Repositories.PatientAttachmentRepository>();

// services
builder.Services.AddScoped<PatientApi.Services.Interfaces.IPatientService, PatientApi.Services.PatientService>();
builder.Services.AddScoped<PatientApi.Services.Interfaces.IMedicalHistoryService, PatientApi.Services.MedicalHistoryService>();
builder.Services.AddScoped<PatientApi.Services.Interfaces.IPatientAttachmentService, PatientApi.Services.PatientAttachmentService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
