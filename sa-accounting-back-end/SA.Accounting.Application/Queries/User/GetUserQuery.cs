using SA.Accounting.Application.Contracts.User.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Queries.User;

public record GetUserQuery : IRequest<Result<UserResponse>>
{
    public int UserId { get; set; }
}
