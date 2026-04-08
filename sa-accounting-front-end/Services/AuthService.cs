using SA.Accounting.WPF.Abstraction;
using SA.Accounting.WPF.Clients.Auth;
using SA.Accounting.WPF.Contracts.Auth.Requests;
using SA.Accounting.WPF.Contracts.Auth.Responses;
using SA.Accounting.WPF.Interfaces;

namespace SA.Accounting.WPF.Services;

public class AuthService(ICacheService cacheService,IAuthClient authClient,IJwtTokenService jwtTokenService) : IAuthService
{
    private readonly ICacheService _cacheService = cacheService;
    private readonly IAuthClient _authClient = authClient;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;

    public async Task<AuthResponse> GetTokenAsync(GetTokenRequest request)
    {
        var authResponse = await this._authClient.GetTokenAsync(request) ;

        var authTokenResponse = _jwtTokenService.DecodeToken(authResponse.Token);

        await _cacheService.SetAsync(KeysConstant.AuthResponse, authResponse, TimeSpan.FromSeconds(authResponse.ExpiresIn));
        await _cacheService.SetAsync(KeysConstant.AuthTokenData, authTokenResponse, TimeSpan.FromSeconds(authResponse.ExpiresIn));

        return authResponse;
    }
}
