using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SA.Accounting.Application.Commands.Role;
using SA.Accounting.Application.Contracts.Role.Responses;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Abstractions.Consts;
using SA.Accounting.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace SA.Accounting.Application.Handlers.CommandsHandler.RoleCommandHandler;

public class CreateRoleCommandHandler(RoleManager<ApplicationRole> roleManager) : IRequestHandler<CreateRoleCommand, Result<RoleDetailResponse>>
{
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

    public async Task<Result<RoleDetailResponse>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {

        if (await _roleManager.FindByNameAsync(request.Name) is not null)
            return Result.Failure<RoleDetailResponse>(RoleErrors.DuplicatedName);

        if (request.Permissions.Except(Permissions.GetAllPermissions()).Any())
            return Result.Failure<RoleDetailResponse>(PermissionErrors.InvalidPermission);

        var appRole = request.Adapt<ApplicationRole>();

        var createResult = await _roleManager.CreateAsync(appRole);

        if(createResult.Succeeded)
        {
            foreach (var permission in request.Permissions)
            {
                var claim = new Claim(Permissions.Type, permission);
                await _roleManager.AddClaimAsync(appRole,claim);
            }

            return Result.Success(appRole.Adapt<RoleDetailResponse>() with { Permissions = request.Permissions });
        }

        var error = createResult.Errors.First();
        return Result.Failure<RoleDetailResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }

}
