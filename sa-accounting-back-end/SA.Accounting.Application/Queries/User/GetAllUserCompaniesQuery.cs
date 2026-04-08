using SA.Accounting.Application.Contracts.Common;
using SA.Accounting.Application.Contracts.Company.Responses;
using SA.Accounting.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Queries.User;

public record GetAllUserCompaniesQuery : IRequest<Result<PaginatedList<CompanyResponse>>>
{
    public int UserId { get; set; }
    public RequestFilters Filters { get; set; } = default!;
}
