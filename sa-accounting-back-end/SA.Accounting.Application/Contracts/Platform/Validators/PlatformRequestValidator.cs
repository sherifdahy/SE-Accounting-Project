using SA.Accounting.Application.Contracts.Platform.Requests;
using SA.Accounting.Application.Contracts.Selector.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Platform.Validators;

public class PlatformRequestValidator : AbstractValidator<PlatformRequest>
{
    public PlatformRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Url)
            .NotEmpty()
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("Invalid URL format");

        RuleFor(x => x.Selectors)
            .NotEmpty()
            .WithMessage("At least one selector is required");

        RuleForEach(x => x.Selectors)
            .SetValidator(new SelectorRequestValidator());
    }
}
