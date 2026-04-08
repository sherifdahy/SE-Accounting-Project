using SA.Accounting.Core.Abstraction;
using SA.Accounting.Infrastructure.Clients.Auth;
using SA.Accounting.Core.Contracts.Auth.Requests;
using SA.Accounting.Core.Contracts.Auth.Responses;
using SA.Accounting.Core.Interfaces;

namespace SA.Accounting.Services;

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

