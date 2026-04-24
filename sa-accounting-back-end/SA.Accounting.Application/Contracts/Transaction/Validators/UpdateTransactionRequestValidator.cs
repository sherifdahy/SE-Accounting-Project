using SA.Accounting.Application.Contracts.Transaction.Requests;
using SA.Accounting.Application.Contracts.TransactionItem.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Transaction.Validators;

public class UpdateTransactionRequestValidator : AbstractValidator<UpdateTransactionRequest>
{
    public UpdateTransactionRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Number)
            .NotEmpty();

        RuleFor(x => x.DateTime)
            .NotEmpty();

        RuleFor(x => x.Items)
            .NotEmpty();

        RuleForEach(x => x.Items)
            .SetValidator(new UpdateTransactionItemRequestValidator());
    }
}
