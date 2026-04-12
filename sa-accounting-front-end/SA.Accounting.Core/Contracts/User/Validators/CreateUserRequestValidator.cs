using FluentValidation;
using SA.Accounting.Core.Consts;
using SA.Accounting.Core.Contracts.User.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Contracts.User.Validators;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(RegexPatterns.Password);

        RuleFor(x => x.PhoneNumber)
            .Matches(RegexPatterns.PhoneNumber)
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

        RuleFor(x => x.Role)
            .NotEmpty();
    }
}
