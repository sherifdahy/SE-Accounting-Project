using SA.Accounting.Application.Contracts.Selector.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Selector.Validators;

public class SelectorRequestValidator : AbstractValidator<SelectorRequest>
{
    public SelectorRequestValidator()
    {
        RuleFor(x => x.Value)
         .NotEmpty()
         .MaximumLength(256)
         .WithMessage("Selector value must not exceed 256 characters");

        RuleFor(x => x.Priority)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x=>x.ContentType).IsInEnum();
    }
}
