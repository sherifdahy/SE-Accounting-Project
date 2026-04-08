using SA.Accounting.Core.Contracts.Auth.Requests;
using SA.Accounting.Core.Contracts.Auth.Responses;

namespace SA.Accounting.Core.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> GetTokenAsync(GetTokenRequest request);
}
