namespace SA.Accounting.Application.Commands.Profile;

public record ChangePasswordCommand : IRequest<Result>
{
    public const string Route = "change-password";
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
