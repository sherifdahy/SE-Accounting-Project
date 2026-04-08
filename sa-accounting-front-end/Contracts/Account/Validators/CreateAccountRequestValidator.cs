using SA.Accounting.WPF.Contracts.Account.Requests;
using SA.Accounting.WPF.ViewModels.Account;

namespace SA.Accounting.WPF.Contracts.Account.Validators;

public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
{
    public CreateAccountRequestValidator()
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