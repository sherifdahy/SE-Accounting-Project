using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Errors;

public static class SelectorErrors
{
    public static Error NotFound => new("Selector.Notfound", "Selector is not Exists", StatusCodes.Status404NotFound);
}
