using Refit;
using SA.Accounting.Core.Contracts.Auth.Requests;
using SA.Accounting.Core.Contracts.Auth.Responses;

namespace SA.Accounting.Infrastructure.Clients.Auth;

public interface IAuthClient
{
    [Post("/auth/get-token")]
    Task<AuthResponse> GetTokenAsync([Body]GetTokenRequest request);
}


