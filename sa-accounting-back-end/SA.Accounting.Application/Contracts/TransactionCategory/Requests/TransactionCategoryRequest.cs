using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.TransactionCategory.Requests;

public record TransactionCategoryRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
