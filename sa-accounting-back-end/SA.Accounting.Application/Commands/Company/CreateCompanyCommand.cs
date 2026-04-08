using SA.Accounting.Application.Contracts.Account.Requests;
using SA.Accounting.Application.Contracts.Company.Responses;
using SA.Accounting.Application.Contracts.Owner.Requests;

namespace SA.Accounting.Application.Commands.Company;

public record CreateCompanyCommand : IRequest<Result<CompanyDetailResponse>>
{
    public string Name { get; set; } = string.Empty;
    public string TaxRegistrationNumber { get; set; } = string.Empty;
    public string TaxFileNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public List<OwnerRequest> Owners { get; set; } = [];
    public List<AccountRequest> Accounts { get; set; } = [];
} ;
