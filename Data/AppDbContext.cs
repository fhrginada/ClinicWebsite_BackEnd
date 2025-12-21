using Microsoft.EntityFrameworkCore;
using PatientApi.Models.Entities;

namespace PatientApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // =========================
        // DbSets
        // =========================
        public DbSet<User> Users { get; set; } = null!;
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
                .HasForeignKey<Patient>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

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
                .HasMany(u => u.Notifications)
                .WithOne(n => n.User)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // =========================
            // Patient
            // =========================
            modelBuilder.Entity<Patient>(b =>
            {
                b.HasKey(p => p.Id);

                b.Property(p => p.UserId).HasMaxLength(100);
                b.Property(p => p.BloodType).HasMaxLength(20);
                b.Property(p => p.Phone).HasMaxLength(50);
                b.Property(p => p.Address).HasMaxLength(500);
                b.Property(p => p.RoleName).HasMaxLength(100);

                b.HasMany(p => p.MedicalHistories)
                    .WithOne(m => m.Patient!)
                    .HasForeignKey(m => m.PatientId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasMany(p => p.Appointments)
                    .WithOne(a => a.Patient)
                    .HasForeignKey(a => a.PatientId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // =========================
            // MedicalHistory
            // =========================
            modelBuilder.Entity<MedicalHistory>(b =>
            {
                b.HasKey(m => m.Id);

                b.Property(m => m.Diagnosis).HasMaxLength(1000);
                b.Property(m => m.Treatment).HasMaxLength(2000);
                b.Property(m => m.AttachmentUrl).HasMaxLength(1000);
            });

            // =========================
            // Doctor
            // =========================
            modelBuilder.Entity<Doctor>(b =>
            {
                b.HasKey(d => d.DoctorId);

                b.HasMany(d => d.Schedules)
                    .WithOne(s => s.Doctor)
                    .HasForeignKey(s => s.DoctorId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasMany(d => d.Appointments)
                    .WithOne(a => a.Doctor)
                    .HasForeignKey(a => a.DoctorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // =========================
            // Nurse
            // =========================
            modelBuilder.Entity<Nurse>(b =>
            {
                b.HasKey(n => n.NurseId);

                b.HasMany(n => n.Schedules)
                    .WithOne(s => s.Nurse)
                    .HasForeignKey(s => s.NurseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // =========================
            // Appointment
            // =========================
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(a => a.AppointmentId);

                entity.HasOne(a => a.Patient)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(a => a.PatientId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(a => a.Doctor)
                    .WithMany(d => d.Appointments)
                    .HasForeignKey(a => a.DoctorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(a => a.Status)
                    .HasDefaultValue(AppointmentStatus.Scheduled);
            });

            // =========================
            // Consultation
            // =========================
            modelBuilder.Entity<Consultation>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.HasOne(c => c.Appointment)
                    .WithOne(a => a.Consultation)
                    .HasForeignKey<Consultation>(c => c.AppointmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(c => c.ConsultationFee)
                    .HasColumnType("decimal(18,2)");

                entity.Property(c => c.IsPaid)
                    .HasDefaultValue(false);
            });

            // =========================
            // AuditLog
            // =========================
            modelBuilder.Entity<AuditLog>(b =>
            {
                b.HasKey(a => a.AuditLogId);
                b.Property(a => a.Action).HasMaxLength(200);
            });

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
                b.Property(m => m.Name).HasMaxLength(200);
                b.Property(m => m.Description).HasMaxLength(1000);
            });

            modelBuilder.Entity<DigitalSignatureToken>(b =>
            {
                b.HasKey(t => t.Id);
                b.Property(t => t.Token).HasMaxLength(500);
            });
        }
    }
}
