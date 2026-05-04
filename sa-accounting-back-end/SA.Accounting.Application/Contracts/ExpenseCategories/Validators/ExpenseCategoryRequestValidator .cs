using SA.Accounting.Application.Contracts.ExpenseCategories.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.ExpenseCategories.Validators;

public class UpsertExpenseCategoryRequestValidator : AbstractValidator<ExpenseCategoryRequest>
{
    public UpsertExpenseCategoryRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);
    }
}
