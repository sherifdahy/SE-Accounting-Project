using FluentValidation;
using SA.Accounting.Core.Contracts.Auth.Requests;

namespace SA.Accounting.Core.Contracts.Auth.Validators;

public class GetTokenValidator : AbstractValidator<GetTokenRequest>
{
    public GetTokenValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("البريد الإلكتروني مطلوب")
            .EmailAddress().WithMessage("صيغة البريد الإلكتروني غير صحيحة");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("كلمة المرور مطلوبة");
    }
}


