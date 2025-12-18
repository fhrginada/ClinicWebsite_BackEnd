using Clinical_project.Data;
using Clinical_project.Middleware;
using Clinical_project.Models;
using Clinical_project.Services.Auth;
using Clinical_project.Services.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Clinical_project.Data.Seed;
using ClinicBackend_Final.Data;

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

// =========================
// Database
// =========================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
        }
    )
);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// =========================
// Identity & JWT
// =========================
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!))
    };
});

var app = builder.Build();

// =========================
// Apply Migrations & Seed
// =========================
using (var scope = app.Services.CreateScope())
{
    // ApplicationDbContext (Identity + Auth)
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
    await DefaultUsersSeeder.SeedRolesAndUsers(roleManager, userManager);

    // AppDbContext (ClinicBackend)
    var clinicDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    clinicDb.Database.Migrate();
}

// =========================
// Middleware & Routing
// =========================
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
