using Mapster;
using Microsoft.AspNetCore.Http;
using SA.Accounting.Application.Contracts.Profile.Responses;
using SA.Accounting.Application.Errors;
using SA.Accounting.Application.Queries.Profile;
using SA.Accounting.Core.Interfaces;
using SA.Accounting.Infrastructure.Extensions;

namespace SA.Accounting.Application.Handlers.QueriesHandler.ProfileQueriesHandler;

public class GetProfileCommandHandler : IRequestHandler<GetProfileQuery, Result<ProfileResponse>>
{
    private readonly IUserServices _userServices;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetProfileCommandHandler(IUserServices userServices,IHttpContextAccessor httpContextAccessor)
    {
        _userServices = userServices;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<Result<ProfileResponse>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var applicationUser =  await _userServices
            .GetProfileAsync(_httpContextAccessor.HttpContext!.User.GetUserId(),cancellationToken);

        if (applicationUser == null)
            return Result.Failure<ProfileResponse>(UserErrors.UserNotAllowed);

        return Result.Success(applicationUser.Adapt<ProfileResponse>());
    }
}
