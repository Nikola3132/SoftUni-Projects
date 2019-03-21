using IT_Firm_DB_Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IT_Firm_DB_Data.EntitiesConfig
{
    internal class EmployeeConfig : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.OwnsOne
              (e => e.Name,
                se =>
                {
                    se.Property(e => e.FirstName).HasColumnName("FirstName");
                    se.Property(e => e.LastName).HasColumnName("LastName");
                }
              );
        }
    }
}
