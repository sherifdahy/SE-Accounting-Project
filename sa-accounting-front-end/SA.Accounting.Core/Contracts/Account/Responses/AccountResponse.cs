using SA.Accounting.Core.Contracts.Platform.Responses;

namespace SA.Accounting.Core.Contracts.Account.Responses;

public class AccountResponse
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public PlatformDetailResponse Platform { get; set; } = default!;
}

