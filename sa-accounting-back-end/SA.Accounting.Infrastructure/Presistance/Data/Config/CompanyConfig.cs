using SA.Accounting.Core.Entities.Companies;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Infrastructure.Presistance.Data.Config;

public class CompanyConfig : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);

        builder.HasIndex(x => x.TaxRegistrationNumber).IsUnique().HasFilter("[IsDeleted] = 0");
        builder.Property(x => x.TaxRegistrationNumber).IsRequired().HasMaxLength(9);
        
        builder.HasIndex(x=> x.TaxFileNumber).IsUnique().HasFilter("[IsDeleted] = 0");
        builder.Property(x=>x.TaxFileNumber).IsRequired().HasMaxLength(10);

        builder.Property(x=>x.Address).HasMaxLength(256);
    }
}
