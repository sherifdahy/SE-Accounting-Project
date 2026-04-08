using SA.Accounting.Core.Abstractions.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SA.Accounting.Infrastructure.Presistance.Data.Config;

public class RoleClaimConfig : IEntityTypeConfiguration<IdentityRoleClaim<int>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<int>> builder)
    {
        var permissions = Permissions.GetAllPermissions();
        
        // admin Claims
        var adminClaims = new List<IdentityRoleClaim<int>>();
        
        for(int i = 0;i < permissions.Count; i++)
        {
            adminClaims.Add(new IdentityRoleClaim<int>
            {
                Id = i + 1,
                ClaimType = Permissions.Type,
                ClaimValue = permissions[i],
                RoleId = DefaultRoles.AdminRoleId,
            });
        }

        builder.HasData(adminClaims);
    }
}
