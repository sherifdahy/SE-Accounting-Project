using Mapster;
using Microsoft.AspNetCore.Http;
using SA.Accounting.Application.Commands.User;
using SA.Accounting.Application.Contracts.User.Responses;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Interfaces;
using SA.Accounting.Infrastructure.Extensions;

namespace SA.Accounting.Application.Handlers.CommandsHandler.ProfileCommandsHandler;

public class GetProfileCommandHandler : IRequestHandler<GetUserProfileCommand, Result<UserProfileResponse>>
{
    private readonly IUserServices _userServices;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetProfileCommandHandler(IUserServices userServices,IHttpContextAccessor httpContextAccessor)
    {
        _userServices = userServices;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<Result<UserProfileResponse>> Handle(GetUserProfileCommand request, CancellationToken cancellationToken)
    {
        var applicationUser =  await _userServices
            .GetProfileAsync(_httpContextAccessor.HttpContext!.User.GetUserId(),cancellationToken);

        if (applicationUser == null)
            return Result.Failure<UserProfileResponse>(UserErrors.UserNotAllowed);

        return Result.Success(applicationUser.Adapt<UserProfileResponse>());
    }
}
