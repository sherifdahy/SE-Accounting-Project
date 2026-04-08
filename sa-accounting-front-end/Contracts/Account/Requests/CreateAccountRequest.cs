using SA.Accounting.WPF.Contracts.Account.Validators;
using SA.Accounting.WPF.Core;

namespace SA.Accounting.WPF.Contracts.Account.Requests;

public sealed class CreateAccountRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int PlatformId { get; set; }
}