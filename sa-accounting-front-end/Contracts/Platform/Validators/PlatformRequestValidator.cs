using FluentValidation;
using SA.Accounting.WPF.Contracts.Platform.Requests;

namespace SA.Accounting.WPF.Contracts.Platform.Validators;

public class PlatformRequestValidator : AbstractValidator<PlatformRequest>
{
    public PlatformRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("اسم المنصة مطلوب")
            .MaximumLength(100).WithMessage("الاسم لا يتجاوز 100 حرف");

        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("رابط المنصة مطلوب")
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("الرابط غير صحيح");

        RuleFor(x => x.ImageUrl)
            .Must(url => string.IsNullOrWhiteSpace(url)
                      || Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("رابط الصورة غير صحيح");
    }
}