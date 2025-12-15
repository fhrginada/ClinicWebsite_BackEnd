using Microsoft.EntityFrameworkCore;
using PatientApi.Models;

namespace PatientApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Patient> Patients { get; set; } = null!;
    public DbSet<MedicalHistory> MedicalHistories { get; set; } = null!;
    public DbSet<PatientAttachment> PatientAttachments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>(b =>
        {
            b.HasKey(p => p.Id);
            b.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
            b.Property(p => p.LastName).HasMaxLength(100);
            b.HasMany(p => p.MedicalHistories).WithOne(m => m.Patient!).HasForeignKey(m => m.PatientId).OnDelete(DeleteBehavior.Cascade);
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
            b.HasOne(a => a.Patient).WithMany(p => p.Attachments).HasForeignKey(a => a.PatientId).OnDelete(DeleteBehavior.Cascade);
        });
    }
}
