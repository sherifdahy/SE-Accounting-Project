using Mapster;
using Microsoft.AspNetCore.Identity;
using SA.Accounting.Application.Contracts.User.Responses;
using SA.Accounting.Application.Errors;
using SA.Accounting.Application.Queries.User;
using SA.Accounting.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Handlers.QueriesHandler.UserQueriesHandler;

public class GetUserQueryHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<GetUserQuery, Result<UserResponse>>
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<Result<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = _userManager.Users.FirstOrDefault(x => x.Id == request.UserId);

        if (user == null)
            return Result.Failure<UserResponse>(UserErrors.NotFound);
        
        var userResponse = user.Adapt<UserResponse>();

        userResponse.Role = (await _userManager.GetRolesAsync(user)).First();

        return Result.Success(userResponse);
    }
}
