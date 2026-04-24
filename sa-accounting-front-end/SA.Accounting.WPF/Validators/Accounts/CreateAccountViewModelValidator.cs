using SA.Accounting.WPF.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.Validators.Accounts;

public class CreateAccountViewModelValidator : AbstractValidator<CreateAccountViewModel>
{
    public CreateAccountViewModelValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty();

        RuleFor(x => x.PlatformId)
            .GreaterThan(0);
    }
}
