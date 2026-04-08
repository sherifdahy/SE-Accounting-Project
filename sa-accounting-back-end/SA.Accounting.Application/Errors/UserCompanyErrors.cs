using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Errors;

public class UserCompanyErrors
{
    public static Error AlreadyAssigned => new("UserCompany.AlreadyAssigned", "Company User is not Exists", StatusCodes.Status404NotFound);
    public static Error NotFound => new("UserCompany.Notfound", "Company User is not Exists", StatusCodes.Status404NotFound);
}
