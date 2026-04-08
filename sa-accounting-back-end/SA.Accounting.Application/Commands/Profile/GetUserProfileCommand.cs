using SA.Accounting.Application.Contracts.User.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.User;

public record GetUserProfileCommand : IRequest<Result<UserProfileResponse>>
{
    public const string Route = "";
}
