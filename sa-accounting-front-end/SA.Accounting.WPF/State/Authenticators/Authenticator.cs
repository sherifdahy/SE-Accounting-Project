using Mapster;
using SA.Accounting.Core.Contracts.Auth.Requests;
using SA.Accounting.Core.Contracts.Auth.Responses;
using SA.Accounting.WPF.Interfaces;

namespace SA.Accounting.WPF.State.Authenticators;

public class Authenticator : IAuthenticator
{
    private readonly IAuthService _authService;
    private readonly IJwtTokenService _jwtTokenService;
    public Authenticator(IAuthService authService,IJwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
        _authService = authService;
    }
    public AuthResponse? CurrentAuthResponse { get; private set; }
    public CurrentUserResponse? CurrentUserResponse { get; private set; }
    public bool IsLoggedIn => CurrentAuthResponse != null;
    public async Task LoginAsync(string email, string password)
    {
        CurrentAuthResponse = await _authService.GetTokenAsync(new GetTokenRequest()
        {
            Email = email,
            Password = password
        });

        CurrentUserResponse = _jwtTokenService.DecodeToken(CurrentAuthResponse.Token).Adapt<CurrentUserResponse>();
    }
    public void Logout()
    {
        CurrentAuthResponse = null!;
        CurrentUserResponse = null!;
    }
}
