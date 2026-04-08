using FluentValidation;
using SA.Accounting.Core.Contracts.Company.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Contracts.Company.Validators;

public class UpdateCompanyRequestValidator : AbstractValidator<UpdateCompanyRequest>
{
    public UpdateCompanyRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("اسم الشركة مطلوب")
            .MaximumLength(256).WithMessage("اسم الشركة يجب ألا يتجاوز 256 حرف");

        RuleFor(x => x.TaxRegistrationNumber)
            .NotEmpty().WithMessage("رقم السجل الضريبي مطلوب")
            .Length(9).WithMessage("رقم السجل الضريبي يجب أن يكون 9 أرقام");

        RuleFor(x => x.TaxFileNumber)
            .NotEmpty().WithMessage("رقم الملف الضريبي مطلوب");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("العنوان مطلوب")
            .MaximumLength(256).WithMessage("العنوان يجب ألا يتجاوز 256 حرف");
    }
}


