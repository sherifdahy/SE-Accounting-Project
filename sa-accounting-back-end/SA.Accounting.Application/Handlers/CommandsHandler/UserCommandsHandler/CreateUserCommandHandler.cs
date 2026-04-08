using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SA.Accounting.Application.Commands.User;
using SA.Accounting.Application.Contracts.User.Responses;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Handlers.CommandsHandler.UserCommandsHandler;

public class CreateUserCommandHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<CreateUserCommand, Result<UserResponse>>
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<Result<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not null)
            return Result.Failure<UserResponse>(UserErrors.DuplicatedEmail);

        if (!string.IsNullOrWhiteSpace(request.SSN) && await _userManager.Users.AnyAsync(x => x.SSN == request.SSN, cancellationToken))
            return Result.Failure<UserResponse>(UserErrors.DuplicateSSN);

        var user = request.Adapt<ApplicationUser>();

        user.UserName = request.Email;

        var createUserResult = await _userManager.CreateAsync(user, request.Password);

        if (createUserResult.Succeeded)
        {
            var addToRoleResult = await _userManager.AddToRoleAsync(user, request.Role);

            if (addToRoleResult.Succeeded)
            {
                return Result.Success(user.Adapt<UserResponse>());
            }
            
            await _userManager.DeleteAsync(user);

            var assignRoleError = addToRoleResult.Errors.First();

            return Result.Failure<UserResponse>(new Error(assignRoleError.Code, assignRoleError.Description, StatusCodes.Status400BadRequest));
        }

        var createError = createUserResult.Errors.First();

        return Result.Failure<UserResponse>(new Error(createError.Code, createError.Description, StatusCodes.Status400BadRequest));
    }
}
