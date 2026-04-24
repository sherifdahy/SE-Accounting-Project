using SA.Accounting.Core.Contracts.Auth.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.Interfaces;

public interface IAuthenticator
{
    AuthResponse? CurrentAuthResponse { get; }
    CurrentUserResponse? CurrentUserResponse { get; }

    event Action StateChanged;
    bool IsLoggedIn { get; }
    Task LoginAsync(string email,string password);
    void Logout();
}
