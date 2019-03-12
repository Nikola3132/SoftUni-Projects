using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using P03_FootballBetting.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P03_FootballBetting.Data
{
    public class FootballBettingContext : DbContext
    {
        public FootballBettingContext(DbContextOptions options) 
            : base(options)
        {
        }

        public FootballBettingContext()
        {
        }

        public DbSet<Bet> Bets { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Config.SqlConnectionStr);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PlayerStatistic>().HasKey(k => new { k.PlayerId, k.GameId });

            builder.Entity<Bet>().HasOne(b => b.User).WithMany(u => u.Bets);
            builder.Entity<Bet>().HasOne(b => b.Game).WithMany(g => g.Bets);

            builder.Entity<Color>().HasMany(c => c.PrimaryKitTeams).WithOne(t => t.PrimaryKitColor);
            builder.Entity<Color>().HasMany(c => c.SecondaryKitTeams).WithOne(t => t.SecondaryKitColor);

            builder.Entity<Country>().HasMany(c => c.Towns).WithOne(t => t.Country);

            builder.Entity<Game>().HasMany(g => g.Bets).WithOne(b => b.Game);
            builder.Entity<Game>().HasMany(g => g.PlayerStatistics).WithOne(ps => ps.Game);
            builder.Entity<Game>().HasOne(g => g.HomeTeam).WithMany(t => t.HomeGames);
            builder.Entity<Game>().HasOne(g => g.AwayTeam).WithMany(t => t.AwayGames);

            builder.Entity<Player>().HasOne(p => p.Position).WithMany(p => p.Players);
            builder.Entity<Player>().HasOne(p => p.Team).WithMany(t => t.Players);
            builder.Entity<Player>().HasMany(p => p.PlayerStatistics).WithOne(ps => ps.Player);

            builder.Entity<Position>().HasMany(p => p.Players).WithOne(p => p.Position);

            builder.Entity<Team>().HasMany(t => t.Players).WithOne(p => p.Team);
            builder.Entity<Team>().HasOne(t => t.Town).WithMany(t => t.Teams);
            builder.Entity<Team>().HasMany(t => t.HomeGames).WithOne(ht => ht.HomeTeam);
            builder.Entity<Team>().HasMany(t => t.AwayGames).WithOne(at => at.AwayTeam);
            builder.Entity<Team>().HasOne(t => t.PrimaryKitColor).WithMany(pk => pk.PrimaryKitTeams);
            builder.Entity<Team>().HasOne(t => t.SecondaryKitColor).WithMany(ck => ck.SecondaryKitTeams);

            builder.Entity<Town>().HasMany(t => t.Teams).WithOne(t => t.Town);
            builder.Entity<Town>().HasOne(t => t.Country).WithMany(t => t.Towns);

            builder.Entity<User>().HasMany(u => u.Bets).WithOne(b => b.User);
        }
    }
}
