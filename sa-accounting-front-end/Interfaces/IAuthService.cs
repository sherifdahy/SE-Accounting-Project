using SA.Accounting.WPF.Contracts.Auth.Requests;
using SA.Accounting.WPF.Contracts.Auth.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> GetTokenAsync(GetTokenRequest request);
}
