using SA.Accounting.Core.Contracts.Auth.Responses;

namespace SA.Accounting.Core.Interfaces;

public interface IJwtTokenService
{
    TokenDataResponse DecodeToken(string token);
}
