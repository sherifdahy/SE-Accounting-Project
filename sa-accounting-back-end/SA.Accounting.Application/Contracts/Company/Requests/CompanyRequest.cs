using SA.Accounting.Application.Contracts.Account.Requests;
using SA.Accounting.Application.Contracts.Owner.Requests;
using SA.Accounting.Core.Entities.Companies;

namespace SA.Accounting.Application.Contracts.Company.Requests;

public class CompanyRequest
{
    public string Name { get; set; } = string.Empty;
    public string TaxRegistrationNumber { get; set; } = string.Empty;
    public string TaxFileNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public List<OwnerRequest> Owners { get; set; } = new List<OwnerRequest>();
    public List<AccountRequest> Accounts { get; set; } = new List<AccountRequest>();
}
