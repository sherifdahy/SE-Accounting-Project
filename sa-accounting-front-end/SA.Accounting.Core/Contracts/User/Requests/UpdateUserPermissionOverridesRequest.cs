using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Contracts.User.Requests;

public record UpdateUserPermissionOverridesRequest
{
    public List<string> DeniedPermissions { get; set; } = [];
}
