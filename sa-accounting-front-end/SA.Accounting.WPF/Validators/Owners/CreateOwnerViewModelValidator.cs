using SA.Accounting.WPF.ViewModels.Owner;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.Validators.Owners;

public class CreateOwnerViewModelValidator : AbstractValidator<CreateOwnerViewModel>
{
    public CreateOwnerViewModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.SSN)
            .NotEmpty();
    }
}
