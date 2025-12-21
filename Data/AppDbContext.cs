using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PatientApi.Models.Entities;
using Clinical_project.Models.Entities;


namespace PatientApi.Data
{
    public class AppDbContext
        : IdentityDbContext<User, IdentityRole<string>, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // =========================
        // DbSets
        // =========================
        public DbSet<Patient> Patients { get; set; } = null!;
        public DbSet<Doctor> Doctors { get; set; } = null!;
        public DbSet<Nurse> Nurses { get; set; } = null!;
        public DbSet<MedicalHistory> MedicalHistories { get; set; } = null!;
        public DbSet<Appointment> Appointments { get; set; } = null!;
        public DbSet<Consultation> Consultations { get; set; } = null!;
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; } = null!;
        public DbSet<NurseSchedule> NurseSchedules { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;
        public DbSet<SystemSettings> SystemSettings { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.PatientProfile)
                .WithOne(p => p.User)
                .HasForeignKey<Patient>(p => p.UserId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.DoctorProfile)
                .WithOne(d => d.User)
                .HasForeignKey<Doctor>(d => d.UserId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.NurseProfile)
                .WithOne(n => n.User)
                .HasForeignKey<Nurse>(n => n.UserId);
        }
    }
}
