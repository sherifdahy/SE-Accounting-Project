using SA.Accounting.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.Validators.Companies;

public class CreateCompanyViewModelValidator : AbstractValidator<CreateCompanyViewModel>
{
    public CreateCompanyViewModelValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
