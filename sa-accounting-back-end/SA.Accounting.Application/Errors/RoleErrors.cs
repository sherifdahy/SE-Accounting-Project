using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Errors;

public static class RoleErrors
{
    public static Error NotFound = new("Roles.NotFound", "Role is Not Found.", StatusCodes.Status404NotFound);

    public static Error DuplicatedName = new("Roles.DuplicatedName", "Role Name is Already exist.", StatusCodes.Status409Conflict);

}
