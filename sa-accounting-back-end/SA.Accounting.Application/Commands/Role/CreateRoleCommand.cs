using SA.Accounting.Application.Contracts.Role.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.Role;

public record CreateRoleCommand : IRequest<Result<RoleDetailResponse>>
{
    public string Name { get; set; } = string.Empty;
    public List<string> Permissions { get; set; } = [];
}
