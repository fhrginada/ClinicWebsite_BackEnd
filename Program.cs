using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PatientApi.Data;
using PatientApi.Models.Entities;
using Clinical_project.Middleware;
using Clinical_project.Services.Auth;
using Clinical_project.Services.Settings;
using Clinical_project.Data.Seed;
using PatientApi.Repositories.Interfaces;
using PatientApi.Repositories;
using PatientApi.Repositories.Implementations;
using PatientApi.Services.Interfaces;
using PatientApi.Services;
using PatientApi.Services.Implementations;
using PatientApi.Repositories.Implementation;

var builder = WebApplication.CreateBuilder(args);

// =========================
// Controllers & Swagger
// =========================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =========================
// Database
// =========================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// =========================
// Identity (INT ✔)
// =========================
builder.Services.AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// =========================
// JWT Authentication
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
            Encoding.UTF8.GetBytes(
                builder.Configuration["JwtSettings:SecretKey"]!
            )
        )
    };
});

// =========================
// Application Services
// =========================
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<RolesService>();
builder.Services.AddScoped<SettingsService>();

// =========================
// Repositories
// =========================
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IMedicalHistoryRepository, MedicalHistoryRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IConsultationRepository, ConsultationRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IDoctorScheduleRepository, DoctorScheduleRepository>();
builder.Services.AddScoped<INurseRepository, NurseRepository>();
builder.Services.AddScoped<INurseScheduleRepository, NurseScheduleRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

// =========================
// Domain Services
// =========================
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IMedicalHistoryService, MedicalHistoryService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IConsultationService, ConsultationService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IDoctorScheduleService, DoctorScheduleService>();
builder.Services.AddScoped<INurseScheduleService, NurseScheduleService>();
builder.Services.AddScoped<IExportService, ExportService>();

// =========================
// CORS
// =========================
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
// Database Migration + Seeder
// =========================
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole<int>>>();

    var userManager = scope.ServiceProvider
        .GetRequiredService<UserManager<User>>();

    var dbContext = scope.ServiceProvider
        .GetRequiredService<AppDbContext>();

    if (app.Environment.IsDevelopment())
    {
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
    }
    else
    {
        dbContext.Database.Migrate();
    }
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
