using FluentValidation;
using SA.Accounting.Core.Consts;
using SA.Accounting.Core.Contracts.User.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Contracts.User.Validators
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty();

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .Matches(RegexPatterns.Password)
                .NotEqual(x => x.CurrentPassword);
        }
    }
}
