using Microsoft.AspNetCore.Identity;
using SA.Accounting.Application.Contracts.Role.Responses;
using SA.Accounting.Application.Errors;
using SA.Accounting.Application.Queries.Role;
using SA.Accounting.Core.Entities.Identity;
using SA.Accounting.Core.Entities.Interfaces;

namespace SA.Accounting.Application.Handlers.QueriesHandler.RoleQueriesHandler;

public class GetRoleQueryHandler(RoleManager<ApplicationRole> roleManager,IUnitOfWork unitOfWork) : IRequestHandler<GetRoleQuery, Result<RoleDetailResponse>>
{
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

    public async Task<Result<RoleDetailResponse>> Handle(GetRoleQuery request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(request.Id.ToString());

        if (role == null)
            return Result.Failure<RoleDetailResponse>(RoleErrors.NotFound);

        var permissions = await _roleManager.GetClaimsAsync(role);

        return Result.Success<RoleDetailResponse>(new RoleDetailResponse()
        {
            Id = role.Id,
            Name = role.Name!,
            Permissions = permissions.Select(x => x.Value).ToList()
        });
    }
}
