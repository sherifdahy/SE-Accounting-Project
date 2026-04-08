using SA.Accounting.Application.Contracts.Common;
using SA.Accounting.Application.Contracts.Owner.Responses;
using SA.Accounting.Core.Abstractions;

namespace SA.Accounting.Application.Queries.Owner;

public record GetAllOwnersQuery : IRequest<Result<PaginatedList<OwnerResponse>>>
{
    public int CompanyId { get; set; }
    public RequestFilters Filters { get; set; } = default!;
    public bool? IncludeDisabled { get; set; }
};

