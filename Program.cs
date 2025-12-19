using Microsoft.EntityFrameworkCore;
using PatientApi.Data;

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

// Register services
// repositories
builder.Services.AddScoped<PatientApi.Repositories.Interfaces.IPatientRepository, PatientApi.Repositories.PatientRepository>();
builder.Services.AddScoped<PatientApi.Repositories.Interfaces.IMedicalHistoryRepository, PatientApi.Repositories.MedicalHistoryRepository>();

// services
builder.Services.AddScoped<PatientApi.Services.Interfaces.IPatientService, PatientApi.Services.PatientService>();
builder.Services.AddScoped<PatientApi.Services.Interfaces.IMedicalHistoryService, PatientApi.Services.MedicalHistoryService>();

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
