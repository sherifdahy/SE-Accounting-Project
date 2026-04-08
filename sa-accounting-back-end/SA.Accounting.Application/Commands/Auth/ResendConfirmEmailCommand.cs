namespace SA.Accounting.Application.Commands.Auth;

public record ResendConfirmEmailCommand : IRequest<Result>
{
    public const string Route = "/api/resend-confirm-email";
    public string Email { get; set; } = string.Empty;
}
