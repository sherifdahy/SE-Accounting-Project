using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.Role;

public record RemoveRoleCommand : IRequest<Result>
{
    public string Name { get; set; }
}
