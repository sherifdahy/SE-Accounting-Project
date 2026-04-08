using Microsoft.AspNetCore.Http;
using System.Net;

namespace SA.Accounting.Application.Errors;
public static class CompanyErrors
{
    public static Error NotFound => new ("Company.Notfound","Company is not Exists",StatusCodes.Status404NotFound);
    public static Error DuplicatedTaxFileNumber=> new ("Company.TaxFileNumber.Duplicate", "This Tax File Number is Alrady Exists.", StatusCodes.Status409Conflict);
    public static Error DuplicatedTaxRegistrationNumber => new ("Company.TaxRegistrationNumber.Duplicate", "This Tax Registration Number is Alrady Exists.", StatusCodes.Status409Conflict);
}
