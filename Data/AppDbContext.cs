using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PatientApi.Models.Entities;
using Clinical_project.Models.Entities;

namespace PatientApi.Data
{
    public class AppDbContext
        : IdentityDbContext<User, IdentityRole<int>, int>
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

        // ========== Prescription Module DbSets ==========
        public DbSet<Prescription> Prescriptions { get; set; } = null!;
        public DbSet<PrescriptionDetail> PrescriptionDetails { get; set; } = null!;
        public DbSet<Medication> Medications { get; set; } = null!;
        public DbSet<DigitalSignatureToken> DigitalSignatureTokens { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // User <-> Profiles
            // =========================
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

            // =========================
            // Prescription Module Config
            // =========================

            modelBuilder.Entity<Prescription>(b =>
            {
                b.HasKey(p => p.PrescriptionId);

                b.HasOne<Consultation>()
                    .WithMany()
                    .HasForeignKey(p => p.ConsultationId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasMany(p => p.Items)
                    .WithOne(i => i.Prescription)
                    .HasForeignKey(i => i.PrescriptionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PrescriptionDetail>(b =>
            {
                b.HasKey(d => d.PrescriptionDetailId);

                b.HasOne(d => d.Medication)
                    .WithMany(m => m.PrescriptionDetails)
                    .HasForeignKey(d => d.MedicationId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Medication>(b =>
            {
                b.HasKey(m => m.MedicationId);

                b.Property(m => m.Name)
                    .HasMaxLength(200);

                b.Property(m => m.Description)
                    .HasMaxLength(1000);
            });

            modelBuilder.Entity<DigitalSignatureToken>(b =>
            {
                b.HasKey(t => t.Id);

                b.Property(t => t.Token)
                    .HasMaxLength(500);
            });
        }
    }
}
