using FluentValidation;
using SA.Accounting.Core.Contracts.User.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Contracts.User.Validators;

public class AssignUserCompaniesRequestValidator : AbstractValidator<AssignUserCompaniesRequest>
{
    public AssignUserCompaniesRequestValidator()
    {
        RuleFor(x => x.CompanyIds)
            .NotEmpty().WithMessage("CompanyIds cannot be empty.")
            .Must(ids => ids.All(id => id > 0))
                .WithMessage("All CompanyIds must be greater than zero.")
            .Must(ids => ids.Distinct().Count() == ids.Count())
                .WithMessage("CompanyIds cannot contain duplicates.");
    }
}