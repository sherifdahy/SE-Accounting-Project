using SA.Accounting.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Infrastructure.Presistance.Data.Config;

public class UserRolePermissionOverrideConfig : IEntityTypeConfiguration<UserRolePermissionOverride>
{
    public void Configure(EntityTypeBuilder<UserRolePermissionOverride> builder)
    {
        builder.HasKey(x => new { x.UserId, x.Value });
    }
}
