using SA.Accounting.Application.Contracts.Account.Responses;

namespace SA.Accounting.Application.Queries.Account;

public record GetAccountQuery : IRequest<Result<AccountResponse>>
{
    public int CompanyId { get; set; }
    public int PlatformId { get; set; }
}
