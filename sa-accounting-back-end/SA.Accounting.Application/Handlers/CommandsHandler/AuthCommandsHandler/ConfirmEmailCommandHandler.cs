using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using SA.Accounting.Application.Commands.Auth;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Identity;
using System.Text;

namespace SA.Accounting.Application.Handlers.CommandsHandler.AuthCommandsHandler;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand,Result>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ConfirmEmailCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    async Task<Result> IRequestHandler<ConfirmEmailCommand, Result>.Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user is null)
            return Result.Failure(UserErrors.InvalidCode);

        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.DuplicatedConfirmation);

        var code = request.Code;

        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        }
        catch(FormatException)
        {
            return Result.Failure(UserErrors.InvalidCode);
        }

        var confirmResult = await _userManager.ConfirmEmailAsync(user, code);

        if(confirmResult.Succeeded)
            return Result.Success();

        var confirmError = confirmResult.Errors.First();

        return Result.Failure(new Error(confirmError.Code,confirmError.Description,StatusCodes.Status400BadRequest));
    }
}
