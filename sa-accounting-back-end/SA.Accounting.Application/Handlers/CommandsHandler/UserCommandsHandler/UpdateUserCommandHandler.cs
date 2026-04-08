using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SA.Accounting.Application.Commands.User;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Identity;

public class UpdateUserCommandHandler(UserManager<ApplicationUser> userManager,RoleManager<ApplicationRole> roleManager) : IRequestHandler<UpdateUserCommand, Result>
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userManager.FindByIdAsync(request.UserId.ToString()) is not ApplicationUser user)
            return Result.Failure(UserErrors.NotFound);

        if (await _roleManager.FindByNameAsync(request.Role) is null)
            return Result.Failure(RoleErrors.NotFound);

        if (await _userManager.Users.AnyAsync(
            x => x.Email == request.Email && x.Id != request.UserId, cancellationToken))
            return Result.Failure(UserErrors.DuplicatedEmail);

        if (!string.IsNullOrWhiteSpace(request.SSN)
            && await _userManager.Users.AnyAsync(
                x => x.SSN == request.SSN && x.Id != request.UserId, cancellationToken))
            return Result.Failure(UserErrors.DuplicateSSN);

        request.Adapt(user);

        user.UserName = request.Email;

        if (!string.IsNullOrEmpty(request.Password))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var passwordResult = await _userManager.ResetPasswordAsync(
                user, token, request.Password);

            if (!passwordResult.Succeeded)
            {
                var pwError = passwordResult.Errors.First();
                return Result.Failure(
                    new Error(pwError.Code, pwError.Description, StatusCodes.Status400BadRequest));
            }
        }

        var updateResult = await _userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            var error = updateResult.Errors.First();
            return Result.Failure(
                new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        var currentRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

        if (currentRole != request.Role)
        {
            if (!string.IsNullOrEmpty(currentRole))
                await _userManager.RemoveFromRoleAsync(user, currentRole);

            await _userManager.AddToRoleAsync(user, request.Role);
        }

        return Result.Success();
    }
}