using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Errors;

public static class OwnerErrors
{
    public static Error NotFound => new ("Owner.Notfound", "Owner is not Exists", StatusCodes.Status404NotFound);
}
