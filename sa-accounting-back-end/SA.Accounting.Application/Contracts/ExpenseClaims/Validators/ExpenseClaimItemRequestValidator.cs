using SA.Accounting.Application.Contracts.ExpenseClaims.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.ExpenseClaims.Validators;

public class ExpenseClaimItemRequestValidator : AbstractValidator<ExpenseClaimItemRequest>
{
    public ExpenseClaimItemRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .When(x => x.Id.HasValue);

        RuleFor(x => x.CompanyId)
            .GreaterThan(0);

        RuleFor(x => x.ExpenseCategoryId)
            .GreaterThan(0);

        RuleFor(x => x.Note)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(x => x.FileUrl)
            .MaximumLength(1000);

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .PrecisionScale(18, 2, true);
    }
}
