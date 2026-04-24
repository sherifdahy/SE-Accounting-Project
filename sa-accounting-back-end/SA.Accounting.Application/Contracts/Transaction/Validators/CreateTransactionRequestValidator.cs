using SA.Accounting.Application.Contracts.Transaction.Requests;
using SA.Accounting.Application.Contracts.TransactionItem.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Transaction.Validators;

public class CreateTransactionRequestValidator : AbstractValidator<CreateTransactionRequest>
{
    public CreateTransactionRequestValidator()
    {
        RuleFor(x => x.Number)
            .NotEmpty();

        RuleFor(x => x.DateTime)
            .NotEmpty();

        RuleFor(x => x.Items)
            .NotEmpty();

        RuleForEach(x => x.Items)
            .SetValidator(new CreateTransactionItemRequestValidator());
    }
}
