using SA.Accounting.WPF.Contracts.Account.Responses;
using SA.Accounting.WPF.Contracts.Owner.Responses;

namespace SA.Accounting.WPF.Contracts.Company.Responses;

public record CompanyDetailResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TaxRegistrationNumber { get; set; } = string.Empty;
    public string TaxFileNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public List<OwnerResponse> Owners { get; set; } = [];
    public List<AccountResponse> Accounts { get; set; } = [];
}
