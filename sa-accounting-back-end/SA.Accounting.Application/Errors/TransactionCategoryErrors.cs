using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Errors;

public static class TransactionCategoryErrors
{
    public static Error NotFound => new("TransactionCategory.Notfound", "Transaction Category is not Exists", StatusCodes.Status404NotFound);
}
