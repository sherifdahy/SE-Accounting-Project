using Microsoft.AspNetCore.Mvc;
using SA.Accounting.Application.Contracts.Owner.Responses;

namespace SA.Accounting.Application.Commands.Owner;

public record CreateOwnerCommand : IRequest<Result<OwnerResponse>>
{
    public int CompanyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SSN { get; set; } = string.Empty;
};