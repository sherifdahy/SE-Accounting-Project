using SA.Accounting.Application.Contracts.Common;
using SA.Accounting.Application.Contracts.Company.Responses;
using SA.Accounting.Core.Abstractions;

namespace SA.Accounting.Application.Queries.Company;

public record GetAllCompaniesQuery: IRequest<Result<PaginatedList<CompanyResponse>>>
{
    public RequestFilters Filters { get; set; } = default!;
    public bool? IncludeDisabled { get; set; }
};

