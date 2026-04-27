using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SA.Accounting.Application.Commands.Role;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Abstractions.Consts;
using SA.Accounting.Core.Entities.Identity;
using System.Security.Claims;

namespace SA.Accounting.Application.Handlers.CommandsHandler.RoleCommandHandler;

public class UpdateRoleCommandHandler(RoleManager<ApplicationRole> roleManager) : IRequestHandler<UpdateRoleCommand, Result>
{
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    public async Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        if (await _roleManager.FindByNameAsync(request.Name) is not ApplicationRole role)
            return Result.Failure(RoleErrors.NotFound);

        if (request.Permissions.Except(Permissions.GetAllPermissions()).Any())
            return Result.Failure(PermissionErrors.InvalidPermission);

        request.Adapt(role);

        var updateResult = await _roleManager.UpdateAsync(role);

        if(updateResult.Succeeded)
        {
            var existsRoleClaims = await _roleManager.GetClaimsAsync(role);

            var newClaims = request.Permissions.Except(existsRoleClaims.Select(x=>x.Value));

            var deletedClaims = existsRoleClaims.Select(x=> x.Value).Except(request.Permissions);

            foreach(var claim in deletedClaims)
            {
                await _roleManager.RemoveClaimAsync(role, new Claim(Permissions.Type, claim));
            }

            foreach(var claim in newClaims)
            {
                await _roleManager.AddClaimAsync(role, new Claim(Permissions.Type, claim));
            }

            return Result.Success();
        }

        var error = updateResult.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }
}
