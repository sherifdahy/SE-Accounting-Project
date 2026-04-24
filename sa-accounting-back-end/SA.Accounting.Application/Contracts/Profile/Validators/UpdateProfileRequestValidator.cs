using SA.Accounting.Application.Abstractions.Consts.RegExp;
using SA.Accounting.Application.Contracts.Profile.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Profile.Validators;

public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();

        RuleFor(x => x.PhoneNumber)
            .Matches(RegexPatterns.PhoneNumber)
            .When(x=> x.PhoneNumber != null);

        RuleFor(x => x.SSN).NotEmpty();
    }
}
