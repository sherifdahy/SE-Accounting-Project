using SA.Accounting.Application.Contracts.TransactionCategory.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Queries.TransactionCategory;

public record GetTransactionCategoryQuery : IRequest<Result<TransactionCategoryResponse>>
{
    public int Id { get; set; }
}
