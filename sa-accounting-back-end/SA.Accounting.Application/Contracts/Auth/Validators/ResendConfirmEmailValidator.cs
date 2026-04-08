using SA.Accounting.Application.Commands.Auth;

namespace SA.Accounting.Application.Contracts.Auth.Validators;

public class ResendConfirmEmailValidator : AbstractValidator<ResendConfirmEmailCommand>
{
    public ResendConfirmEmailValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}
