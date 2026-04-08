using SA.Accounting.WPF.Contracts.Auth.Validators;
using SA.Accounting.WPF.Core;

namespace SA.Accounting.WPF.Contracts.Auth.Requests;

public class GetTokenRequest 
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}