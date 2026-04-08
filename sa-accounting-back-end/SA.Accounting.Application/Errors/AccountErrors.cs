using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Errors;

public static class AccountErrors
{
    public static Error NotFound => new("Account.NotFound", "Email is not Exists.", StatusCodes.Status404NotFound);
    public static Error DuplicatedAccount => new("Account.Duplicated", "Platfrom is Already Exists for this Company.", StatusCodes.Status409Conflict);
}
