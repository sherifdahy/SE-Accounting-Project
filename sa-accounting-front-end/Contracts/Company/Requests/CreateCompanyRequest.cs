using System.Collections.ObjectModel;
using SA.Accounting.WPF.Contracts.Account.Requests;
using SA.Accounting.WPF.Contracts.Company.Validators;
using SA.Accounting.WPF.Contracts.Owner.Requests;
using SA.Accounting.WPF.Core;

namespace SA.Accounting.WPF.Contracts.Company.Requests;

public sealed class CreateCompanyRequest
{
    public string Name { get; set; } = string.Empty;
    public string TaxRegistrationNumber { get; set; } = string.Empty;
    public string TaxFileNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public List<CreateOwnerRequest> Owners { get; set; } = [];
    public List<CreateAccountRequest> Accounts { get; set; } = [];
}