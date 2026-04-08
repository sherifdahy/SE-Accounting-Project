using SA.Accounting.Core.Contracts.Platform.Responses;

namespace SA.Accounting.Core.Contracts.Account.Responses;

partial class AccountDetailResponse
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public PlatformResponse Platform { get; set; } = default!;
}

