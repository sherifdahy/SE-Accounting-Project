using SA.Accounting.WPF.Contracts.Owner.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.Contracts.Owner.Validators;

public class UpdateOwnerRequestValidator : AbstractValidator<UpdateOwnerRequest>
{
    public UpdateOwnerRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("اسم المالك مطلوب")
            .MaximumLength(256).WithMessage("اسم المالك يجب ألا يتجاوز 256 حرف");

        RuleFor(x => x.SSN)
            .NotEmpty().WithMessage("رقم الهوية مطلوب")
            .Length(14).WithMessage("رقم الهوية يجب أن يكون 14 رقم");
    }
}