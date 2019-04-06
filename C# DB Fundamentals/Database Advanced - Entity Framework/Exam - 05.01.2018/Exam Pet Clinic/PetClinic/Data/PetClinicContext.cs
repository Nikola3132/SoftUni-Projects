namespace PetClinic.Data
{
    using Microsoft.EntityFrameworkCore;
    using PetClinic.Models;

    public class PetClinicContext : DbContext
    {
        public PetClinicContext() { }

        public PetClinicContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Animal> Animals { get; set; }
        public DbSet<AnimalAid> AnimalAids { get; set; }
        public DbSet<Passport> Passports { get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<ProcedureAnimalAid> ProceduresAnimalAids { get; set; }
        public DbSet<Vet> Vets { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Passport
            builder.Entity<Passport>()
             .HasOne(a => a.Animal)
             .WithOne(x => x.Passport)
             .HasForeignKey<Animal>(b => b.PassportSerialNumber);

            //Vet
            builder.Entity<Vet>()
                .HasIndex(v => v.PhoneNumber)
                .IsUnique();

            builder.Entity<Vet>()
                .HasMany(v => v.Procedures)
                .WithOne(p => p.Vet);

            //AnimalAid
            builder.Entity<AnimalAid>()
                .HasIndex(aa => aa.Name)
                .IsUnique();

            builder.Entity<AnimalAid>()
                .HasMany(aa => aa.AnimalAidProcedures)
                .WithOne(aap => aap.AnimalAid);

            //Animal
            builder.Entity<Animal>()
                .HasMany(a => a.Procedures)
                .WithOne(p => p.Animal);

            builder.Entity<Animal>()
                .HasOne(a => a.Passport)
                .WithOne(p => p.Animal);

            //Procedure
            builder.Entity<Procedure>()
                .HasMany(p => p.ProcedureAnimalAids)
                .WithOne(paa => paa.Procedure);

            builder.Entity<Procedure>()
                .HasOne(p => p.Vet)
                .WithMany(v => v.Procedures);

            builder.Entity<Procedure>()
                .HasOne(p => p.Animal)
                .WithMany(a => a.Procedures);

            //ProcedureAnimalAid
            builder.Entity<ProcedureAnimalAid>()
                .HasKey(paa => new { paa.AnimalAidId, paa.ProcedureId });

            builder.Entity<ProcedureAnimalAid>()
                .HasOne(paa => paa.AnimalAid)
                .WithMany(aa => aa.AnimalAidProcedures);

            builder.Entity<ProcedureAnimalAid>()
                .HasOne(paa => paa.Procedure)
                .WithMany(p => p.ProcedureAnimalAids);
        }
    }
}
