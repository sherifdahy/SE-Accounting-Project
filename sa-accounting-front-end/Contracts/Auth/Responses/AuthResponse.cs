namespace SA.Accounting.WPF.Contracts.Auth.Responses;

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }

}
