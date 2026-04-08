namespace SA.Accounting.WPF.Contracts.User.Requests;

public record ChangePasswordRequest
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
