using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SA.Accounting.Application.Commands.Role;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Handlers.CommandsHandler.RoleCommandHandler;

public class RemoveRoleCommandHandler(RoleManager<ApplicationRole> roleManager) : IRequestHandler<RemoveRoleCommand, Result>
{
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    public async Task<Result> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
    {
        if (await _roleManager.FindByNameAsync(request.Name) is not ApplicationRole role)
            return Result.Failure(RoleErrors.NotFound);

        var deletedResult = await _roleManager.DeleteAsync(role);

        if(!deletedResult.Succeeded)
        {
            var error = deletedResult.Errors.First();
            return Result.Failure(new Error(error.Code,error.Description,StatusCodes.Status400BadRequest));
        }

        return Result.Success();
    }
}
