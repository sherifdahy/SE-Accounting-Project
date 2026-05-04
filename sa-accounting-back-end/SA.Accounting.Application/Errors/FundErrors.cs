using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Errors;

public static class FundErrors
{
    public static Error NotFound => new Error("Fund.NotFound", "Fund not Found", StatusCodes.Status404NotFound);
}
