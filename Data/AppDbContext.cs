using Microsoft.EntityFrameworkCore;
using ClinicBackend_Final.Models.Entities;

namespace ClinicBackend_Final.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Existing
        public DbSet<Patient> Patients { get; set; } = null!;
        public DbSet<MedicalHistory> MedicalHistories { get; set; } = null!;

        // 🔴 Prescription System 
        public DbSet<Prescription> Prescriptions { get; set; } = null!;
        public DbSet<PrescriptionItem> PrescriptionItems { get; set; } = null!;
        public DbSet<Medication> Medications { get; set; } = null!;
        public DbSet<DigitalSignatureToken> DigitalSignatureTokens { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
            // Prescription
            // =========================
            modelBuilder.Entity<Prescription>(b =>
            {
                b.HasKey(p => p.Id);
                b.Property(p => p.Status).HasMaxLength(50);

                b.HasMany(p => p.Items)
                 .WithOne(i => i.Prescription!)
                 .HasForeignKey(i => i.PrescriptionId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasMany(p => p.Tokens)
                 .WithOne(t => t.Prescription!)
                 .HasForeignKey(t => t.PrescriptionId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // =========================
            // PrescriptionItem
            // =========================
            modelBuilder.Entity<PrescriptionItem>(b =>
            {
                b.HasKey(i => i.Id);
                b.Property(i => i.Dose).HasMaxLength(100);
                b.Property(i => i.Frequency).HasMaxLength(100);
            });

            // =========================
            // Medication
            // =========================
            modelBuilder.Entity<Medication>(b =>
            {
                b.HasKey(m => m.Id);
                b.Property(m => m.Name).HasMaxLength(200);
                b.Property(m => m.Strength).HasMaxLength(100);
            });

            // =========================
            // DigitalSignatureToken
            // =========================
            modelBuilder.Entity<DigitalSignatureToken>(b =>
            {
                b.HasKey(t => t.Id);
                b.Property(t => t.Token).HasMaxLength(500);
            });
        }
    }
}
