using Mapster;
using Microsoft.AspNetCore.Http;
using SA.Accounting.Application.Commands.User;
using SA.Accounting.Core.Interfaces;
using SA.Accounting.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Handlers.CommandsHandler.ProfileCommandsHandler;

public class UpdateProfileHandler(IUserServices userServices, IHttpContextAccessor contextAccessor) : IRequestHandler<UpdateProfileCommand, Result>
{
    private readonly IUserServices _userServices = userServices;
    private readonly IHttpContextAccessor _contextAccessor = contextAccessor;
    public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var applicationUser = await _userServices.GetProfileAsync(_contextAccessor.HttpContext!.User.GetUserId(), cancellationToken);

        request.Adapt(applicationUser);

        var result = await _userServices.UpdateProfileAsync(applicationUser,cancellationToken);

        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code,error.Description,StatusCodes.Status400BadRequest));
    }
}
