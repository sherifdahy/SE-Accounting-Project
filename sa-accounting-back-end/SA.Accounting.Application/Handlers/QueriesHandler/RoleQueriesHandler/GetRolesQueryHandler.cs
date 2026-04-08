using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SA.Accounting.Application.Contracts.Role.Responses;
using SA.Accounting.Application.Queries.Role;
using SA.Accounting.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Handlers.QueriesHandler.RoleCommandsHandler;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, Result<List<RoleResponse>>>
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    public GetRolesQueryHandler(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }
    public async Task<Result<List<RoleResponse>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _roleManager.Roles.Where(x=> !x.IsDeleted).ProjectToType<RoleResponse>().ToListAsync(cancellationToken);

        return Result.Success(roles);
    }
}
