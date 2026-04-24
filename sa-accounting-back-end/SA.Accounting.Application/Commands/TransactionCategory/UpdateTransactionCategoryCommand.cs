using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.TransactionCategory;

public record UpdateTransactionCategoryCommand : IRequest<Result>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
