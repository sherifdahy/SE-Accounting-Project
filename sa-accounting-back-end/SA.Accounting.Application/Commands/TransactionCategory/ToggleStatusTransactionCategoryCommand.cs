using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.TransactionCategory;

public record ToggleStatusTransactionCategoryCommand : IRequest<Result>
{
    public int Id { get; set; }
}
