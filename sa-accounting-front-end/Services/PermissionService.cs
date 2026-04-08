using SA.Accounting.WPF.Abstraction;
using SA.Accounting.WPF.Contracts.Auth.Responses;
using SA.Accounting.WPF.Interfaces;

namespace SA.Accounting.WPF.Services;
public class PermissionService : IPermissionService
{
    private readonly ICacheService _cacheService;

    public PermissionService(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    private async Task<TokenDataResponse?> GetTokenAsync()
    {
        return await _cacheService.GetAsync<TokenDataResponse>(KeysConstant.AuthTokenData);
    }

    public async Task<bool> HasPermissionAsync(string permission)
    {
        var token = await GetTokenAsync();
        if (token?.Permissions == null) return false;

        return token.Permissions.Any(p =>
            p.Equals(permission, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<string> GetUserEmailAsync()
    {
        var token = await GetTokenAsync();
        return token?.Email ?? "";
    }
}
