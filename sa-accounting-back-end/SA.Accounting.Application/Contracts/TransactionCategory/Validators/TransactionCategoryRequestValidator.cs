using SA.Accounting.Application.Contracts.TransactionCategory.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.TransactionCategory.Validators;

public class TransactionCategoryRequestValidator : AbstractValidator<TransactionCategoryRequest>
{
    public TransactionCategoryRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

    }
}
