using SA.Accounting.Core.Entities.Companies;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Infrastructure.Presistance.Data.Config;

public class AccountConfig : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.Property(x => x.Email).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Password).IsRequired().HasMaxLength(256);

        builder.Property(x => x.CompanyId).IsRequired();
        builder.Property(x=>x.PlatformId).IsRequired();
    }
}
