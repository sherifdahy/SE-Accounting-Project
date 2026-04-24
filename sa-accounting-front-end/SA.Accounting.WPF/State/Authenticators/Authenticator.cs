using Mapster;
using SA.Accounting.Core.Contracts.Auth.Requests;
using SA.Accounting.Core.Contracts.Auth.Responses;
using SA.Accounting.WPF.Interfaces;

namespace SA.Accounting.WPF.State.Authenticators;

public class Authenticator : IAuthenticator
{
    private readonly IAuthService _authService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IAccountStore _accountStore;

    public Authenticator(IAuthService authService,IJwtTokenService jwtTokenService,IAccountStore accountStore)
    {
        _jwtTokenService = jwtTokenService;
        _accountStore = accountStore;
        _authService = authService;
    }

    public event Action StateChanged;
    public AuthResponse? CurrentAuthResponse { get; private set; }
    public CurrentUserResponse CurrentUserResponse { get => _accountStore.CurrentUserResponse; private set { _accountStore.CurrentUserResponse = value; StateChanged?.Invoke(); } }
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
