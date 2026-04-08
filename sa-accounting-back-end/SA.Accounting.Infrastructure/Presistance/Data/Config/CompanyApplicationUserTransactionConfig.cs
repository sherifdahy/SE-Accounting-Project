using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Infrastructure.Presistance.Data.Config;

public class CompanyApplicationUserTransactionConfig : IEntityTypeConfiguration<CompanyUserTransaction>
{
    public void Configure(EntityTypeBuilder<CompanyUserTransaction> builder)
    {
        builder.HasKey(x => new { x.CompanyId, x.UserId, x.TransactionId });
    }
}
