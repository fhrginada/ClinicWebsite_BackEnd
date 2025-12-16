using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Clinical_project.Models;
using Clinical_project.Models.Entities; 
namespace Clinical_project.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

      
        public DbSet<Patient> Patients { get; set; } 
        public DbSet<SystemSettings> SystemSettings { get; set; } 

    }
}