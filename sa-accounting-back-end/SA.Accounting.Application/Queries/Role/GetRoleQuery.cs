using SA.Accounting.Application.Contracts.Role.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Queries.Role;

public record GetRoleQuery : IRequest<Result<RoleDetailResponse>>
{
    public int Id { get; set; }
}
