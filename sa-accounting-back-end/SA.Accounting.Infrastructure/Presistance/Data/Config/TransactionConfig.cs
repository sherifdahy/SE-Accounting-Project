using SA.Accounting.Core.Entities.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Infrastructure.Presistance.Data.Config;

public class TransactionConfig : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasOne(t => t.User)
            .WithMany(u => u.Transactions)   
            .HasForeignKey(t => t.UserId);

        builder.HasOne(t => t.CreatedBy)
            .WithMany()                       
            .HasForeignKey(t => t.CreatedById);

        builder.HasOne(t => t.UpdatedBy)
            .WithMany()                     
            .HasForeignKey(t => t.UpdatedById);
    }
}
