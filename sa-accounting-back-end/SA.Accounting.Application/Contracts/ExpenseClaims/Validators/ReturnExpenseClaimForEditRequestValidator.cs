using SA.Accounting.Application.Contracts.ExpenseClaims.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.ExpenseClaims.Validators;

public class ReturnExpenseClaimForEditRequestValidator : AbstractValidator<ReturnExpenseClaimForEditRequest>
{
    public ReturnExpenseClaimForEditRequestValidator()
    {
        RuleFor(x => x.Reason)
            .NotEmpty()
            .MaximumLength(1000);
    }
}
