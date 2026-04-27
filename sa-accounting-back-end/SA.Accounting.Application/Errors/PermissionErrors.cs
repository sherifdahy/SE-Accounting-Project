using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Errors;

public static class PermissionErrors
{
    public static Error InvalidPermission => new Error("Permission.InvalidPermission", "Invalid Permission Selected", StatusCodes.Status400BadRequest);
}
