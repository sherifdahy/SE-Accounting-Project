using SA.Accounting.Application.Contracts.Account.Responses;

namespace SA.Accounting.Application.Commands.Account;

public record CreateAccountCommand : IRequest<Result<AccountResponse>>
{
    public int CompanyId { get; set; }
    public int PlatformId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
