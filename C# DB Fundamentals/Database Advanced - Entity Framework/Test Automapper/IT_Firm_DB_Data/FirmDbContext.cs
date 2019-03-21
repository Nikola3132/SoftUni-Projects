using IT_Firm_DB_Data.EntitiesConfig;
using IT_Firm_DB_Models.Entities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IT_Firm_DB_Data
{
    public class FirmDbContext : DbContext
    {
        public FirmDbContext(DbContextOptions options) 
            : base(options)
        {
        }

        public FirmDbContext()
        {
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (optionsBuilder.IsConfigured == false)
            //{
            //    base.OnConfiguring(optionsBuilder);
            //    optionsBuilder.UseSqlServer(@"Server=DESKTOP-JU304LN\SQLEXPRESS;Database=Firm;Integrated Security=true");
            //}
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new EmployeeConfig());
        }
    }
}
