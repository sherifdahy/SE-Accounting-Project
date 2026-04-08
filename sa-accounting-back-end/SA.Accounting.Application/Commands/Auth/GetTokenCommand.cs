
using SA.Accounting.Application.Contracts.Auth.Responses;

namespace SA.Accounting.Application.Commands.Auth;

public record GetTokenCommand : IRequest<Result<AuthResponse>>
{
    public const string Route = "/auth/get-token"; 
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public GetTokenCommand(string email,string password)
    {
        Email = email;
        Password = password;
    }
}
