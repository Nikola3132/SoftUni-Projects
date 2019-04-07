namespace Cinema.Data
{
    using Cinema.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class CinemaContext : DbContext
    {
        public CinemaContext()  { }

        public CinemaContext(DbContextOptions options)
            : base(options)   { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Projection> Projections { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

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
            //Customer
            builder.Entity<Customer>().
                HasMany(c => c.Tickets)
                .WithOne(t => t.Customer);

            //Hall
            builder.Entity<Hall>()
                .HasMany(h => h.Projections)
                .WithOne(p => p.Hall);

            builder.Entity<Hall>()
                .HasMany(h => h.Seats)
                .WithOne(s => s.Hall);

            //Movie
            builder.Entity<Movie>()
                .HasMany(m => m.Projections)
                .WithOne(p => p.Movie);

            //Projection
            builder.Entity<Projection>()
                .HasOne(p => p.Movie)
                .WithMany(m => m.Projections);

            builder.Entity<Projection>()
                .HasOne(p => p.Hall)
                .WithMany(h => h.Projections);

            builder.Entity<Projection>()
                .HasMany(p => p.Tickets)
                .WithOne(t => t.Projection);

            //Seat
            builder.Entity<Seat>()
                .HasOne(s => s.Hall)
                .WithMany(h => h.Seats);

            //Ticket
            builder.Entity<Ticket>()
                .HasOne(t => t.Customer)
                .WithMany(c => c.Tickets);

            builder.Entity<Ticket>()
                .HasOne(t => t.Projection)
                .WithMany(p => p.Tickets);
        }
    }
}