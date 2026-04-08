namespace SA.Accounting.Application.Contracts.Account.Requests;

public class AccountRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int PlatformId { get; set; }
}
