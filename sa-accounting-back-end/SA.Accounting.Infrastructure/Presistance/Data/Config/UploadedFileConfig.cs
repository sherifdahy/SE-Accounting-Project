using SA.Accounting.Core.Entities.Files;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Infrastructure.Presistance.Data.Config;

public class UploadedFileConfig : IEntityTypeConfiguration<UploadedFile>
{
    public void Configure(EntityTypeBuilder<UploadedFile> builder)
    {
        builder.Property(x => x.ExpenseClaimItemId).IsRequired();

        builder.Property(x => x.FileName).HasMaxLength(250);
        builder.Property(x => x.StoredFileName).HasMaxLength(250);
        builder.Property(x => x.ContentType).HasMaxLength(50);
        builder.Property(x => x.FileExtension).HasMaxLength(10);
    }
}
