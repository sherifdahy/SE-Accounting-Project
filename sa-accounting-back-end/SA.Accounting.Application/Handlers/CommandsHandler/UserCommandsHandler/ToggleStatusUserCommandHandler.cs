using Microsoft.AspNetCore.Identity;
using SA.Accounting.Application.Commands.User;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Identity;

namespace SA.Accounting.Application.Handlers.CommandsHandler.UserCommandsHandler;

public class ToggleStatusUserCommandHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<ToggleStatusUserCommand, Result>
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    public async Task<Result> Handle(ToggleStatusUserCommand request, CancellationToken cancellationToken)
    {
        var user = _userManager.Users.FirstOrDefault(u => u.Id == request.UserId);

        if (user is null)
            return Result.Failure(UserErrors.NotFound);

        user.IsDisabled = !user.IsDisabled;

        await _userManager.UpdateAsync(user);

        return Result.Success();
    }
}
