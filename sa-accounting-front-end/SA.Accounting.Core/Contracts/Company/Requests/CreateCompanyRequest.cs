using System.Collections.ObjectModel;
using SA.Accounting.Core.Contracts.Account.Requests;
using SA.Accounting.Core.Contracts.Company.Validators;
using SA.Accounting.Core.Contracts.Owner.Requests;
using SA.Accounting.Core;

namespace SA.Accounting.Core.Contracts.Company.Requests;

public sealed class CreateCompanyRequest
{
    public string Name { get; set; } = string.Empty;
    public string TaxRegistrationNumber { get; set; } = string.Empty;
    public string TaxFileNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public List<CreateOwnerRequest> Owners { get; set; } = [];
    public List<CreateAccountRequest> Accounts { get; set; } = [];
}
