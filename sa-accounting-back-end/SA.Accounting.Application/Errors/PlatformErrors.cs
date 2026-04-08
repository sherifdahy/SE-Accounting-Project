using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Errors;

public static class PlatformErrors
{
    public static Error NotFound => new ("Platform.NotFound","Platform is not Exists.",StatusCodes.Status404NotFound);
    public static Error DuplicatedName => new("Platform.Duplicated.Name", "Platform with this Name is Already Exists.", StatusCodes.Status409Conflict);

}
