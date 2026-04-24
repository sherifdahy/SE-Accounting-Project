using SA.Accounting.Application.Contracts.TransactionCategory.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.TransactionCategory;

public class CreateTransactionCategoryCommand : IRequest<Result<TransactionCategoryResponse>>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
