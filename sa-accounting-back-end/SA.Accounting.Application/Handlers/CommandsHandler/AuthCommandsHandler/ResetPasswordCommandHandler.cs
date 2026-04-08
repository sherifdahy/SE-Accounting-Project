using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SA.Accounting.Application.Commands.Auth;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Identity;
using SA.Accounting.Core.Interfaces;

namespace SA.Accounting.Application.Handlers.CommandsHandler.AuthCommandsHandler;

public class ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager,IAuthServices authServices) : IRequestHandler<ResetPasswordCommand, Result>
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IAuthServices _authServices = authServices;

    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var applicationUser = await _userManager.FindByEmailAsync(request.Email);

        if (applicationUser is null || !applicationUser.EmailConfirmed)
            return Result.Failure(UserErrors.InvalidCode);

        var result = await _authServices.ResetPasswordAsync(applicationUser, request.Code, request.NewPassword);

        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status401Unauthorized));
    }
}
