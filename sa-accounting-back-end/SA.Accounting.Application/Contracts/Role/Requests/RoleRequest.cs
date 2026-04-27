using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Role.Requests;

public record RoleRequest
{
    public string Name { get; set; } = string.Empty;
    public List<string> Permissions { get; set; } = [];
}
