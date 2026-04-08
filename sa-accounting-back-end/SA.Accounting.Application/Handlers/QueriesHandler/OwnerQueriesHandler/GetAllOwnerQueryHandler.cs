using Mapster;
using SA.Accounting.Application.Contracts.Company.Responses;
using SA.Accounting.Application.Contracts.Owner.Responses;
using SA.Accounting.Application.Queries.Owner;
using SA.Accounting.Core.Abstractions;
using SA.Accounting.Core.Entities.Companies;
using SA.Accounting.Core.Entities.Interfaces;
using System.Linq.Expressions;

namespace SA.Accounting.Application.Handlers.QueriesHandler.OwnerQueriesHandler;

public class GetAllOwnerQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllOwnersQuery, Result<PaginatedList<OwnerResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result<PaginatedList<OwnerResponse>>> Handle(GetAllOwnersQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Owner, bool>> query = x => (string.IsNullOrEmpty(request.Filters.SearchValue) || x.Name.Contains(request.Filters.SearchValue)) && (request.IncludeDisabled == true || x.IsDeleted == false);

        var count = await _unitOfWork.Owners.CountAsync(query);

        var owners = await _unitOfWork.Owners.FindAllAsync(query, (request.Filters.PageNumber - 1) * request.Filters.PageSize, request.Filters.PageSize, request.Filters.SortColumn, request.Filters.SortDirection, cancellationToken);

        return Result.Success(PaginatedList<OwnerResponse>.Create(owners.Adapt<List<OwnerResponse>>(), count, request.Filters.PageNumber, request.Filters.PageSize));

    }
}
