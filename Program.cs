using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PatientApi.Data;
using PatientApi.Models.Entities;
using Clinical_project.Middleware;
using Clinical_project.Services.Auth;
using Clinical_project.Services.Settings;
using Clinical_project.Data.Seed;

var builder = WebApplication.CreateBuilder(args);

// =========================
// Controllers & Swagger
// =========================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =========================
// Application Services
// =========================
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<RolesService>();
builder.Services.AddScoped<SettingsService>();

// =========================
// Database
// =========================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// =========================
// Identity (✔ int)
// =========================
builder.Services.AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// =========================
// JWT
// =========================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!)
        )
    };
});

// =========================
// Repositories & Services
// =========================
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

// =========================
// Seeder
// =========================
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<string>>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    dbContext.Database.Migrate();
    await DefaultUsersSeeder.SeedRolesAndUsers(roleManager, userManager);
}

// =========================
// Middleware
// =========================
app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<LocalizationMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
