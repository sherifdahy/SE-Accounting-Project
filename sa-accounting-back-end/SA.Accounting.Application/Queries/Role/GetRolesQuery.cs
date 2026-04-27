using SA.Accounting.Application.Contracts.Role.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Queries.Role;

public record GetRolesQuery : IRequest<Result<List<RoleResponse>>>
{
}
