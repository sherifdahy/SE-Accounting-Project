using SA.Accounting.WPF.Contracts.Account.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.Contracts.Account.Validators;

public class UpdateAccountRequestValidator : AbstractValidator<UpdateAccountRequest>
{
    public UpdateAccountRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("البريد الإلكتروني مطلوب")
            .EmailAddress().WithMessage("البريد الإلكتروني غير صحيح")
            .MaximumLength(256).WithMessage("البريد الإلكتروني يجب ألا يتجاوز 256 حرف");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("كلمة المرور مطلوبة")
            .MaximumLength(256).WithMessage("كلمة المرور يجب ألا تتجاوز 256 حرف");
    }
}
