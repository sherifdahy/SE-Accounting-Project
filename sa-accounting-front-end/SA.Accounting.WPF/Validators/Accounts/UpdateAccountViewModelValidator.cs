using SA.Accounting.WPF.ViewModels.Account;

namespace SA.Accounting.WPF.Validators;

public class UpdateAccountViewModelValidator : AbstractValidator<UpdateAccountViewModel>
{
    public UpdateAccountViewModelValidator()
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