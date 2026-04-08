using SA.Accounting.Application.Commands.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Auth.Validators;

public class ForgetPasswordValidator : AbstractValidator<ForgetPasswordCommand>
{
    public ForgetPasswordValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}
