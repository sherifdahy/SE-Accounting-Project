using SA.Accounting.Core.Entities.Custodies;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Infrastructure.Presistance.Data.Config;

public class CustodyConfig : IEntityTypeConfiguration<Custody>
{
    public void Configure(EntityTypeBuilder<Custody> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Number)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Note)
            .HasMaxLength(500);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(x => x.Number)
            .IsUnique();

        builder.HasIndex(x => x.UserId)
            .IsUnique()
            .HasFilter("[IsActive] = 1");

        builder.HasOne(x => x.CreatedBy)
            .WithMany()
            .HasForeignKey(x => x.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.UpdatedBy)
            .WithMany()
            .HasForeignKey(x => x.UpdatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
