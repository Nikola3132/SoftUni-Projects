using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P03_SalesDatabase.Data
{
    public class SalesContext : DbContext
    {
        public SalesContext(DbContextOptions options) 
            : base(options)
        {
        }

        public SalesContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.SqlConnectionStr);
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Store> Stores { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Sales)
                .WithOne(s => s.Customer);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Sales)
                .WithOne(s => s.Product);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Store)
                .WithMany(s => s.Sales);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Customer)
                .WithMany(s => s.Sales);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Product)
                .WithMany(s => s.Sales);

            
            modelBuilder.Entity<Product>
                ().Property(d => d.Description).HasDefaultValue("No description");

            modelBuilder.Entity<Store>()
                .HasMany(s => s.Sales)
                .WithOne(s => s.Store);
            
            modelBuilder.Entity<Sale>()
                .Property(e => e.Date).HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Product>()
                .Property(n => n.Name).IsUnicode(true);

            modelBuilder.Entity<Customer>()
                .Property(n => n.Name).IsUnicode(true);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Email).IsUnicode(false);

            modelBuilder.Entity<Store>()
                .Property(n => n.Name).IsUnicode(true);
        }
    }
}
