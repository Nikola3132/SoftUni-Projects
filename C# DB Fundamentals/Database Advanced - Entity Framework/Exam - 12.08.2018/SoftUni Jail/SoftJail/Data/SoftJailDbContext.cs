namespace SoftJail.Data
{
	using Microsoft.EntityFrameworkCore;
    using SoftJail.Data.Models;

    public class SoftJailDbContext : DbContext
	{
		public SoftJailDbContext()
		{
		}

		public SoftJailDbContext(DbContextOptions options)
			: base(options)
		{
		}

        public DbSet<Cell> Cells { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Mail> Mails { get; set; }
        public DbSet<Officer> Officers { get; set; }
        public DbSet<OfficerPrisoner> OfficersPrisoners { get; set; }
        public DbSet<Prisoner> Prisoners { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder
					.UseSqlServer(Configuration.ConnectionString);
			}
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
            //OfficerPrisoner
            builder.Entity<OfficerPrisoner>()
                .HasKey(op => new { op.OfficerId, op.PrisonerId });

            builder.Entity<OfficerPrisoner>()
                .HasOne(op => op.Officer).WithMany(o => o.OfficerPrisoners)
                 .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<OfficerPrisoner>()
                .HasOne(op => op.Prisoner).WithMany(p => p.PrisonerOfficers)
                 .OnDelete(DeleteBehavior.Restrict);

            //Cell
            builder.Entity<Cell>().HasOne(c => c.Department).WithMany(d => d.Cells);
            builder.Entity<Cell>().HasMany(c => c.Prisoners).WithOne(p => p.Cell);

            //Department
            builder.Entity<Department>().HasMany(d => d.Cells).WithOne(c => c.Department);

            //Mall
            builder.Entity<Mail>().HasOne(m => m.Prisoner).WithMany(m => m.Mails);

            //Officer
            builder.Entity<Officer>().HasOne(o => o.Department);
            builder.Entity<Officer>().HasMany(o => o.OfficerPrisoners).WithOne(op => op.Officer);

            //Prisoner
            builder.Entity<Prisoner>().HasOne(p => p.Cell).WithMany(c => c.Prisoners);
            builder.Entity<Prisoner>().HasMany(p => p.Mails).WithOne(m => m.Prisoner);
            builder.Entity<Prisoner>().HasMany(p => p.PrisonerOfficers).WithOne(m => m.Prisoner);
        }
	}
}