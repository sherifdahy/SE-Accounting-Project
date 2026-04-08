using SA.Accounting.Application.Contracts.Account.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Account.Validators;

public class UpdateAccountValidator : AbstractValidator<UpdateAccountRequest>
{
    public UpdateAccountValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256).WithMessage("Email Address is required and must not exceed 256 characters."); ;
        RuleFor(x => x.Password).NotEmpty().MaximumLength(256).WithMessage("Password is required and must not exceed 256 characters."); ;
    }
}
