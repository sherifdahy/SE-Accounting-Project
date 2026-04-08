using SA.Accounting.Core.Entities.Companies;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Infrastructure.Presistance.Data.Config;

public class OwnerConfig : IEntityTypeConfiguration<Owner>
{
    public void Configure(EntityTypeBuilder<Owner> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);

        builder.Property(x=>x.SSN).IsRequired().HasMaxLength(14);
    }
}
