using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.PermissionOverride.Requests;

public record UpdateUserPermissionOverridesRequest
{
    public List<string> DeniedPermissions { get; set; } = [];
}
