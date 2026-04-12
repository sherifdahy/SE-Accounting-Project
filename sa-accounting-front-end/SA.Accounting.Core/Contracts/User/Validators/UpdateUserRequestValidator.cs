using FluentValidation;
using SA.Accounting.Core.Consts;
using SA.Accounting.Core.Contracts.User.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Contracts.User.Validators;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(RegexPatterns.Password)
            .When(p=> !string.IsNullOrEmpty(p.Password));

        RuleFor(x => x.PhoneNumber)
            .Matches(RegexPatterns.PhoneNumber)
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));
    }
}
