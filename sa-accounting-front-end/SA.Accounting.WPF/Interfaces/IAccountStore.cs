using SA.Accounting.Core.Contracts.Auth.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.Interfaces;

public interface IAccountStore
{
    public CurrentUserResponse CurrentUserResponse { get; set; } 
}
