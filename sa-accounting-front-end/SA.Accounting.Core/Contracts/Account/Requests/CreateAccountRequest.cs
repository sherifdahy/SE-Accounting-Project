using SA.Accounting.Core.Contracts.Account.Validators;
using SA.Accounting.Core;

namespace SA.Accounting.Core.Contracts.Account.Requests;

public sealed class CreateAccountRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int PlatformId { get; set; }
}
