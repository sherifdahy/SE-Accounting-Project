using SA.Accounting.Application.Contracts.Role.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Queries.Role;

public class GetRolesQuery : IRequest<Result<List<RoleResponse>>>
{
    public const string Route = "roles";
}
