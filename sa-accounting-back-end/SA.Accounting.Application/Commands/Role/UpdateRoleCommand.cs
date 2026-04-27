using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.Role;

public record UpdateRoleCommand : IRequest<Result>
{
    public string Name { get; set; } = string.Empty;
    public List<string> Permissions { get; set; } = [];
}
