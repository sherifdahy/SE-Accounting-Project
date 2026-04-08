using FluentValidation;
using SA.Accounting.Core.Contracts.Selector.Requests;

namespace SA.Accounting.Core.Contracts.Selector.Validators;

public class SelectorRequestValidator : AbstractValidator<SelectorRequest>
{
    public SelectorRequestValidator()
    {
        RuleFor(x => x.Value)
            .NotEmpty().WithMessage("القيمة مطلوبة");

        RuleFor(x => x.ContentType)
            .IsInEnum().WithMessage("نوع المحتوى غير صالح");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("نوع المحدد غير صالح");

        RuleFor(x => x.Priority)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("الاولية يجب انت تكون اكبر من صفر");
    }
}


