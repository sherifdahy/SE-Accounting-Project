using SA.Accounting.Application.Contracts.Account.Requests;
using SA.Accounting.Application.Contracts.Owner.Requests;

namespace SA.Accounting.Application.Commands.Company;

public record UpdateCompanyCommand : IRequest<Result>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TaxRegistrationNumber { get; set; } = string.Empty;
    public string TaxFileNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public List<UpdateOwnerRequest> Owners { get; set; } = [];
    public List<UpdateAccountRequest> Accounts { get; set; } = [];
}
