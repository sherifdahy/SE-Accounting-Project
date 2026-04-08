using SA.Accounting.Core.Contracts.Auth.Validators;
using SA.Accounting.Core;

namespace SA.Accounting.Core.Contracts.Auth.Requests;

public class GetTokenRequest 
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
