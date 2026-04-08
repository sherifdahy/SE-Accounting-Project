using SA.Accounting.Core.Entities.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Infrastructure.Presistance.Data.Config;

public class TransactionItemConfig : IEntityTypeConfiguration<TransactionItem>
{
    public void Configure(EntityTypeBuilder<TransactionItem> builder)
    {
        builder.Property(x => x.Note).HasMaxLength(1000);
        builder.Property(x => x.FileUrl).HasMaxLength(500);
    }
}
