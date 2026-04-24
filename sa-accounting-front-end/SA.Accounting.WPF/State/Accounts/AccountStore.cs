using SA.Accounting.Core.Contracts.Auth.Responses;
using SA.Accounting.WPF.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.State.Accounts;

public class AccountStore : IAccountStore
{
    private CurrentUserResponse _currentUserResponse;
    public CurrentUserResponse CurrentUserResponse { get => _currentUserResponse; set => _currentUserResponse = value; }
}
