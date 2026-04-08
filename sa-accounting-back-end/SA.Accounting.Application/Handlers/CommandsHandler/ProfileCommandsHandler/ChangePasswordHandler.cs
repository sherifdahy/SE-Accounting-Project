using Microsoft.AspNetCore.Http;
using SA.Accounting.Application.Commands.Profile;
using SA.Accounting.Core.Interfaces;
using SA.Accounting.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Handlers.CommandsHandler.ProfileCommandsHandler;

public class ChangePasswordHandler(IUserServices userServices, IHttpContextAccessor httpContextAccessor) : IRequestHandler<ChangePasswordCommand, Result>
{
    private readonly IUserServices _userServices = userServices;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var result = await _userServices
            .ChangePasswordAsync(_httpContextAccessor
                                        .HttpContext!
                                        .User
                                        .GetUserId(),
                                 request.CurrentPassword,
                                 request.NewPassword,
                                 cancellationToken);

        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }
}
