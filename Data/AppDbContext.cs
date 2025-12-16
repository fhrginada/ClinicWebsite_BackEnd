using Microsoft.EntityFrameworkCore;
using PatientApi.Models.Entities;


namespace PatientApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        
        public DbSet<Patient> Patients { get; set; } = null!;
        public DbSet<MedicalHistory> MedicalHistories { get; set; } = null!;
        public DbSet<PatientAttachment> PatientAttachments { get; set; } = null!;

        
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Doctor> Doctors { get; set; } = null!;
        public DbSet<Nurse> Nurses { get; set; } = null!;
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; } = null!;
        public DbSet<NurseSchedule> NurseSchedules { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ===== Patient Config =====
            modelBuilder.Entity<Patient>(b =>
            {
                b.HasKey(p => p.Id);
                b.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
                b.Property(p => p.LastName).HasMaxLength(100);

                b.HasMany(p => p.MedicalHistories)
                    .WithOne(m => m.Patient!)
                    .HasForeignKey(m => m.PatientId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasMany(p => p.Attachments)
                    .WithOne(a => a.Patient)
                    .HasForeignKey(a => a.PatientId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<MedicalHistory>(b =>
            {
                b.HasKey(m => m.Id);
                b.Property(m => m.Diagnosis).HasMaxLength(1000);
                b.Property(m => m.Treatment).HasMaxLength(2000);
                b.Property(m => m.Notes).HasMaxLength(2000);
            });

            modelBuilder.Entity<PatientAttachment>(b =>
            {
                b.HasKey(a => a.Id);
                b.Property(a => a.FileName).IsRequired().HasMaxLength(500);
                b.Property(a => a.ContentType).HasMaxLength(200);
                b.Property(a => a.Path).IsRequired().HasMaxLength(1000);
            });

            // ===== User <-> Doctor / Nurse =====
            modelBuilder.Entity<User>()
                .HasOne(u => u.DoctorProfile)
                .WithOne(d => d.User)
                .HasForeignKey<Doctor>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.NurseProfile)
                .WithOne(n => n.User)
                .HasForeignKey<Nurse>(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // ===== Doctor =====
            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.Schedules)
                .WithOne(s => s.Doctor)
                .HasForeignKey(s => s.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);

            // ===== Nurse =====
            modelBuilder.Entity<Nurse>()
                .HasMany(n => n.Schedules)
                .WithOne(s => s.Nurse)
                .HasForeignKey(s => s.NurseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
