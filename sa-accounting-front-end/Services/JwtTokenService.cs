using SA.Accounting.WPF.Contracts.Auth.Responses;
using SA.Accounting.WPF.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SA.Accounting.WPF.Services;

public class JwtTokenService : IJwtTokenService
{
    public TokenDataResponse DecodeToken(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var tokenData = new TokenDataResponse
            {
                UserId = GetClaimValue(jwtToken, "sub") ?? GetClaimValue(jwtToken, "userId"),
                Email = GetClaimValue(jwtToken, "email"),
                Roles = GetRolesFromToken(jwtToken),
                Permissions = GetPermissionsFromToken(jwtToken),
                ExpiresAt = jwtToken.ValidTo
            };

            return tokenData;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("فشل في فك تشفير الـ Token", ex);
        }
    }
    private List<string> GetRolesFromToken(JwtSecurityToken token)
    {
        var roles = token.Claims
            .Where(c => c.Type == "roles" || c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

        if (roles.Any())
            return roles;

        if (token.Payload.TryGetValue("roles", out var rolesObj))
        {
            if (rolesObj is IEnumerable<object> rolesArray)
            {
                return rolesArray.Select(r => r.ToString()!).ToList();
            }
        }

        return new List<string>();
    }

    private string? GetClaimValue(JwtSecurityToken token, string claimType)
    {
        return token.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
    }

    private List<string> GetPermissionsFromToken(JwtSecurityToken token)
    {
        var permissionsClaim = token.Claims
            .Where(c => c.Type == "permissions" || c.Type == "permission")
            .Select(c => c.Value)
            .ToList();

        if (permissionsClaim.Any())
            return permissionsClaim;

        var permissionsJson = GetClaimValue(token, "permissions");
        if (!string.IsNullOrEmpty(permissionsJson))
        {
            try
            {
                return System.Text.Json.JsonSerializer.Deserialize<List<string>>(permissionsJson)
                       ?? new List<string>();
            }
            catch { }
        }

        var multiplePermissions = token.Claims
            .Where(c => c.Type.EndsWith("permission", StringComparison.OrdinalIgnoreCase))
            .Select(c => c.Value)
            .ToList();

        return multiplePermissions;
    }
}

