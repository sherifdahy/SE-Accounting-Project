using SA.Accounting.Core.Entities.Platforms;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Infrastructure.Presistance.Data.Config;

public class SelectorConfig : IEntityTypeConfiguration<Selector>
{
    public void Configure(EntityTypeBuilder<Selector> builder)
    {
        builder.Property(x => x.Value).IsRequired().HasMaxLength(256);
        builder.Property(x=>x.Type).IsRequired();
        builder.Property(x => x.ContentType).IsRequired();
    }
}
