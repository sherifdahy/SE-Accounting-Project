using Mapster;
using SA.Accounting.Application.Contracts.TransactionCategory.Responses;
using SA.Accounting.Application.Errors;
using SA.Accounting.Application.Queries.TransactionCategory;
using SA.Accounting.Core.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Handlers.QueriesHandler.TransactionCategoryQueriesHandler;

public class GetTransactionCategoryQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetTransactionCategoryQuery, Result<TransactionCategoryResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result<TransactionCategoryResponse>> Handle(GetTransactionCategoryQuery request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.TransactionCategories.GetByIdAsync(request.Id, cancellationToken);

        if (entity is null)
            return Result.Failure<TransactionCategoryResponse>(TransactionCategoryErrors.NotFound);

        return Result.Success(entity.Adapt<TransactionCategoryResponse>());
    }
}
