using Microsoft.EntityFrameworkCore;
using PatientApi.Models.Entities;


namespace PatientApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Patient> Patients { get; set; } = null!;
        public DbSet<MedicalHistory> MedicalHistories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<MedicalHistory>(b =>
            {
                b.HasKey(m => m.Id);
                b.Property(m => m.Diagnosis).HasMaxLength(1000);
                b.Property(m => m.Treatment).HasMaxLength(2000);
                b.Property(m => m.AttachmentUrl).HasMaxLength(1000);
            });
        }
    }
}
