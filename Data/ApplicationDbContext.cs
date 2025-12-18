using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Clinical_project.Models;
using Clinical_project.Models.Entities; // ⬅️ 1. يجب التأكد من وجود هذا السطر

namespace Clinical_project.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // كيان المريض (Patient)
        public DbSet<Patient> Patients { get; set; }

        // ⬅️ 2. يجب التأكد من وجود هذا السطر لمهام العضو 1
        public DbSet<SystemSettings> SystemSettings { get; set; }
    }
}
