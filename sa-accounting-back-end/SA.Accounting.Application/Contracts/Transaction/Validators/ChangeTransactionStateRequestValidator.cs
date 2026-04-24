using SA.Accounting.Application.Contracts.Transaction.Requests;
using SA.Accounting.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Transaction.Validators;

public class ChangeTransactionStateRequestValidator : AbstractValidator<ChangeTransactionStateRequest>
{
    public ChangeTransactionStateRequestValidator()
    {
        RuleFor(x => x.NewState)
            .IsInEnum();

        RuleFor(x => x.Note)
            .NotEmpty()
            .When(x => x.NewState == TransactionState.Rejected)
            .WithMessage("يجب إدخال سبب الرفض");
    }
}
