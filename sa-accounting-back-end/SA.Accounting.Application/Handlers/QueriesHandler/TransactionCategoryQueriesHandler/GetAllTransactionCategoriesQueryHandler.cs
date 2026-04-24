using Mapster;
using SA.Accounting.Application.Contracts.TransactionCategory.Responses;
using SA.Accounting.Application.Queries.Transaction;
using SA.Accounting.Application.Queries.TransactionCategory;
using SA.Accounting.Core.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Handlers.QueriesHandler.TransactionCategoryQueriesHandler;

public class GetAllTransactionCategoriesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllTransactionCategoriesQuery, Result<List<TransactionCategoryResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result<List<TransactionCategoryResponse>>> Handle(GetAllTransactionCategoriesQuery request, CancellationToken cancellationToken)
    {
        var transactionCategories = await _unitOfWork.TransactionCategories.FindAllAsync(x => request.IncludeDisabled == true || !x.IsDeleted, [], cancellationToken);

        return Result.Success(transactionCategories.Adapt<List<TransactionCategoryResponse>>());
    }
}
