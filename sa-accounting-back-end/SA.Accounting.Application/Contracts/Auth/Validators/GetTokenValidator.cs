using SA.Accounting.Application.Abstractions.Consts.RegExp;
using SA.Accounting.Application.Commands.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Auth.Validators;

public class GetTokenValidator : AbstractValidator<GetTokenCommand>
{
    public GetTokenValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(RegexPatterns.Password);
    }
}
