using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.ExpenseCategories.Responses;

public record ExpenseCategoryResponse(
    int Id,
    string Name,
    bool RequiresAttachment,
    bool IsDisabled
);
