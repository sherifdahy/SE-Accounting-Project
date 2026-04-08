using Microsoft.AspNetCore.Mvc;
using SA.Accounting.Application.Contracts.Account.Responses;
using SA.Accounting.Application.Contracts.Common;
using SA.Accounting.Core.Abstractions;

namespace SA.Accounting.Application.Queries.Account;

public record GetAllAccountQuery : IRequest<Result<List<AccountResponse>>>
{
    public int CompanyId { get; set; }
    public bool? IncludeDisabled { get; set; }
}
