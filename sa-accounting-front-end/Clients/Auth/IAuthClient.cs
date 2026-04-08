using Refit;
using SA.Accounting.WPF.Contracts.Auth.Requests;
using SA.Accounting.WPF.Contracts.Auth.Responses;

namespace SA.Accounting.WPF.Clients.Auth;

public interface IAuthClient
{
    [Post("/auth/get-token")]
    Task<AuthResponse> GetTokenAsync([Body]GetTokenRequest request);
}
