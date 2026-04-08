using SA.Accounting.Core.Abstractions.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SA.Accounting.Infrastructure.Presistance.Data.Config;

public class UserRoleConfig : IEntityTypeConfiguration<IdentityUserRole<int>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<int>> builder)
    {
        builder.HasData(new IdentityUserRole<int>
        {
            RoleId = DefaultRoles.AdminRoleId,
            UserId = DefaultUsers.AdminId,
        });
    }
}
