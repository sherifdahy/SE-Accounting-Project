using SA.Accounting.Application.Contracts.TransactionItem.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.TransactionItem.Validators;

public class CreateTransactionItemRequestValidator : AbstractValidator<CreateTransactionItemRequest>
{
    public CreateTransactionItemRequestValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.CompanyId)
            .GreaterThan(0);

        RuleFor(x => x.TransactionCategoryId)
            .GreaterThan(0);
    }
}
