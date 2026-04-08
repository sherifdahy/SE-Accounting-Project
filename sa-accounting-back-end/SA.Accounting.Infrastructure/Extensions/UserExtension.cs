using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace SA.Accounting.Infrastructure.Extensions;

public static class UserExtension
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        if(user == null) throw new ArgumentNullException(nameof(user));

        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        int.TryParse(userId, out int id);

        return id;
    }
}
