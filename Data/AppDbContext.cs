using System.Collections.Generic;
using System.Reflection.Emit;
using ClinicalBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicalBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Prescription> Prescriptions { get; set; }
        // public DbSet<Medication> Medications { get; set; } // لو موجود

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // يمكنك إضافة configurations هنا لو حابب
        }
    }
}