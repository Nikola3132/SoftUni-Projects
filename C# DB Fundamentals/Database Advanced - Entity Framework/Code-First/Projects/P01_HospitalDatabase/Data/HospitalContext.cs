using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase.Data
{
    public class HospitalContext : DbContext
    {
        public HospitalContext(DbContextOptions options) : base(options)
        {
        }

        public HospitalContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.SqlConnectionStr);
        }

        public DbSet<Diagnose> Diagnoses { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientMedicament> PatientMedicaments { get; set; }
        public DbSet<Visitation> Visitations { get; set; }
        public DbSet<Doctor> Doctors { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Prescriptions)
                .WithOne(pm => pm.Patient);

            modelBuilder.Entity<Patient>()
                .HasMany(v => v.Visitations)
                .WithOne(v => v.Patient);

            modelBuilder.Entity<Patient>()
                .HasMany(d => d.Diagnoses)
                .WithOne(d => d.Patient);

            modelBuilder.Entity<Diagnose>()
                .HasOne(d => d.Patient)
                .WithMany(p => p.Diagnoses);

            modelBuilder.Entity<Medicament>()
                .HasMany(m => m.Prescriptions)
                .WithOne(p => p.Medicament);


            modelBuilder.Entity<Visitation>()
                .HasOne(v => v.Patient)
                .WithMany(p => p.Visitations);

            modelBuilder.Entity<PatientMedicament>()
                .HasKey(k => new { k.PatientId, k.MedicamentId });


            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.Visitations)
                .WithOne(v => v.Doctor);
            
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.Property(e => e.FirstName).IsUnicode(true);
                entity.Property(e => e.LastName).IsUnicode(true);
                entity.Property(e => e.Address).IsUnicode(true);
                entity.Property(e => e.Email).IsUnicode(false);
            });

            modelBuilder.Entity<Visitation>(e =>
            {
                e.Property(c => c.Comments).IsUnicode(true);
            });

            modelBuilder.Entity<Diagnose>(d =>
            {
                d.Property(n => n.Name).IsUnicode(true);
                d.Property(c => c.Comments).IsUnicode(true);
            });

            modelBuilder.Entity<Medicament>(m =>
            {
                m.Property(n => n.Name).IsUnicode(true);
            });

            modelBuilder.Entity<Doctor>(d =>
            {
                d.Property(n => n.Name).IsUnicode(true);
                d.Property(s => s.Specialty).IsUnicode(true);
                d.Property(e => e.Email).HasMaxLength(30).IsUnicode(false);
            });
        }
    }
}
