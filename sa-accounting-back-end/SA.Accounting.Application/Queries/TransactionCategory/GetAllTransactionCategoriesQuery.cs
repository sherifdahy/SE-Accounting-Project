using SA.Accounting.Application.Contracts.TransactionCategory.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Queries.TransactionCategory;

public record GetAllTransactionCategoriesQuery : IRequest<Result<List<TransactionCategoryResponse>>>
{
    public bool? IncludeDisabled { get; set; }
}
