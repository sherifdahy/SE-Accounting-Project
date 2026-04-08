using SA.Accounting.Core.Entities.Platforms;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Infrastructure.Presistance.Data.Config;

internal class PlatformConfig : IEntityTypeConfiguration<Platform>
{
    public void Configure(EntityTypeBuilder<Platform> builder)
    {
        builder.HasIndex(x => x.Name).IsUnique().HasFilter("[IsDeleted] = 0");
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        
        builder.Property(x => x.Url).IsRequired().HasMaxLength(1000);
    }
}
