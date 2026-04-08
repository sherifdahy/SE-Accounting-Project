using Microsoft.AspNetCore.Identity;
using SA.Accounting.Application.Commands.Auth;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Identity;
using SA.Accounting.Core.Interfaces;

namespace SA.Accounting.Application.Handlers.CommandsHandler.AuthCommandsHandler;

public class ForgetPasswordCommandHandler(IAuthServices authServices, UserManager<ApplicationUser> userManager) : IRequestHandler<ForgetPasswordCommand, Result>
{
    private readonly IAuthServices _authServices = authServices;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<Result> Handle(ForgetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Success();

        if (!user.EmailConfirmed)
            return Result.Failure(UserErrors.EmailNotConfirmed);

        await _authServices.SendResetPasswordEmail(user);

        return Result.Success();
    }
}
