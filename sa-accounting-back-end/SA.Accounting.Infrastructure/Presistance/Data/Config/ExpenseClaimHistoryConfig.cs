using SA.Accounting.Core.Entities.ExpenseClaims;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Infrastructure.Presistance.Data.Config;

public class ExpenseClaimHistoryConfig : IEntityTypeConfiguration<ExpenseClaimHistory>
{
    public void Configure(EntityTypeBuilder<ExpenseClaimHistory> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FromState)
            .IsRequired();

        builder.Property(x => x.ToState)
            .IsRequired();

        builder.Property(x => x.Note)
            .HasMaxLength(1000);

        builder.HasOne(x => x.UpdatedBy)
            .WithMany()
            .HasForeignKey(x=>x.UpdatedById);

        builder.HasOne(x => x.CreatedBy)
            .WithMany()
            .HasForeignKey(x => x.CreatedById);

        builder.HasIndex(x => x.ExpenseClaimId);

        builder.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_ExpenseClaimHistory_FromState_Valid",
                "[FromState] IN (1, 2, 3, 4, 5, 6, 7, 8, 9)");

            t.HasCheckConstraint(
                "CK_ExpenseClaimHistory_ToState_Valid",
                "[ToState] IN (1, 2, 3, 4, 5, 6, 7, 8, 9)");

            t.HasCheckConstraint(
                "CK_ExpenseClaimHistory_FromState_NotEqual_ToState",
                "[FromState] <> [ToState]");
        });
    }
}
