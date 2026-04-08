using SA.Accounting.Application.Abstractions.Consts.RegExp;
using SA.Accounting.Application.Commands.Profile;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.User.Validators
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty();

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .Matches(RegexPatterns.Password)
                .NotEqual(x=>x.CurrentPassword);
        }
    }
}
