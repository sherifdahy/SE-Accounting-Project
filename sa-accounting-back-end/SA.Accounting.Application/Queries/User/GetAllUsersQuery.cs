using SA.Accounting.Application.Contracts.Common;
using SA.Accounting.Application.Contracts.User.Responses;
using SA.Accounting.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Queries.User;

public record GetAllUsersQuery : IRequest<Result<PaginatedList<UserResponse>>>
{
    public bool IncludeDisabled { get; set; }
    public RequestFilters Filters { get; set; } = default!;
}
