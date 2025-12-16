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
builder.Services.AddScoped<PatientApi.Repositories.Interfaces.IPatientRepository, PatientApi.Repositories.PatientRepository>();
builder.Services.AddScoped<PatientApi.Repositories.Interfaces.IMedicalHistoryRepository, PatientApi.Repositories.MedicalHistoryRepository>();
builder.Services.AddScoped<PatientApi.Repositories.Interfaces.IPatientAttachmentRepository, PatientApi.Repositories.PatientAttachmentRepository>();

// services
builder.Services.AddScoped<PatientApi.Services.Interfaces.IPatientService, PatientApi.Services.PatientService>();
builder.Services.AddScoped<PatientApi.Services.Interfaces.IMedicalHistoryService, PatientApi.Services.MedicalHistoryService>();
builder.Services.AddScoped<PatientApi.Services.Interfaces.IPatientAttachmentService, PatientApi.Services.PatientAttachmentService>();

var app = builder.Build();

// Apply pending EF Core migrations on startup
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
