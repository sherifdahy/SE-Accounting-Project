using SA.Accounting.WPF.Contracts.Auth.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.Interfaces;

public interface IJwtTokenService
{
    TokenDataResponse DecodeToken(string token);
}
