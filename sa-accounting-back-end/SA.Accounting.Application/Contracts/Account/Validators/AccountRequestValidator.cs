using SA.Accounting.Application.Contracts.Account.Requests;

namespace SA.Accounting.Application.Contracts.Account.Validators;

public class AccountRequestValidator : AbstractValidator<AccountRequest>
{
    public AccountRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256).WithMessage("Email Address is required and must not exceed 256 characters."); ;
        RuleFor(x => x.Password).NotEmpty().MaximumLength(256).WithMessage("Password is required and must not exceed 256 characters."); ;
    }
}
