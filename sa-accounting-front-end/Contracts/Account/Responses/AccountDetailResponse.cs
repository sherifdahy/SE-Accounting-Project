using SA.Accounting.WPF.Contracts.Platform.Responses;

namespace SA.Accounting.WPF.Contracts.Account.Responses;

partial class AccountDetailResponse
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public PlatformResponse Platform { get; set; } = default!;
}
