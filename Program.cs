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
