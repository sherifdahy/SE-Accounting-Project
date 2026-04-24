using SA.Accounting.WPF.ViewModels.Owner;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.Validators.Owners;

public class UpdateOwnerViewModelValidator : AbstractValidator<UpdateOwnerViewModel>
{
    public UpdateOwnerViewModelValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.SSN).NotEmpty();
    }
}
