using SA.Accounting.Core.Entities.Identity;

namespace SA.Accounting.Services.Authentication;

public interface IJWTProvider
{
    (string token, int expiresIn) GeneratedToken(ApplicationUser applicationUser, IEnumerable<string> applicationRoles, IEnumerable<string> permissions);
    int? ValidateToken(string token);
}
