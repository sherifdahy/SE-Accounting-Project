using Mapster;
using SA.Accounting.Application.Contracts.Company.Responses;
using SA.Accounting.Application.Queries.Company;
using SA.Accounting.Core.Abstractions;
using SA.Accounting.Core.Entities.Companies;
using SA.Accounting.Core.Entities.Interfaces;
using System.Linq.Expressions;

namespace SA.Accounting.Application.Handlers.QueriesHandler.CompanyQueriesHandler;

public class GetAllCompaniesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllCompaniesQuery, Result<PaginatedList<CompanyResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result<PaginatedList<CompanyResponse>>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Company, bool>> query = x => (string.IsNullOrEmpty(request.Filters.SearchValue) || x.Name.Contains(request.Filters.SearchValue)) && (request.IncludeDisabled == true || x.IsDeleted == false);

        var count = await _unitOfWork.Companies.CountAsync(query);

        var companies = await _unitOfWork.Companies.FindAllAsync(query,(request.Filters.PageNumber - 1) * request.Filters.PageSize,request.Filters.PageSize,request.Filters.SortColumn,request.Filters.SortDirection,cancellationToken);

        return Result.Success(PaginatedList<CompanyResponse>.Create(companies.Adapt<List<CompanyResponse>>(),count,request.Filters.PageNumber, request.Filters.PageSize));
    }
}
