using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Contracts.User.Responses;

public record UserPermissionOverridesResponse
{
    public List<string> Denied { get; set; } = [];
    public List<string> Default { get; set; } = [];
}
