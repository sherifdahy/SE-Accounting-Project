using SA.Accounting.Application.Contracts.Owner.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Owner.Validators;

public class OwnerValidator : AbstractValidator<OwnerRequest>
{
    public OwnerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(256)
            .WithMessage("Owner name is required and must not exceed 256 characters.");

        RuleFor(x => x.SSN)
            .NotEmpty()
            .Length(14)
            .WithMessage("SSN must be exactly 14 characters.");
    }
}
