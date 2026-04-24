using SA.Accounting.Core.Consts;
using SA.Accounting.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.Validators.Auth;

public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
{
    public LoginViewModelValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(RegexPatterns.Password).WithMessage("كلمة المرور يجب أن تكون 8 أحرف على الأقل وتحتوي على حرف كبير وصغير ورقم ورمز خاص.");
    }
}
