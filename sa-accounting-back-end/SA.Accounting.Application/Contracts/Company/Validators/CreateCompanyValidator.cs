using SA.Accounting.Application.Contracts.Account.Validators;
using SA.Accounting.Application.Contracts.Company.Requests;
using SA.Accounting.Application.Contracts.Owner.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Company.Validators;

public class CreateCompanyValidator : AbstractValidator<CompanyRequest>
{
    public CreateCompanyValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(256)
            .WithMessage("Name is required and must not exceed 256 characters.");

        RuleFor(x => x.TaxRegistrationNumber)
            .NotEmpty()
            .Length(9)
            .WithMessage("Tax Registration Number must be exactly 9 characters.");

        RuleFor(x => x.TaxFileNumber)
            .NotEmpty()
            .Length(10)
            .WithMessage("Tax File Number must be exactly 10 characters.");

        RuleFor(x => x.Address)
            .MaximumLength(256)
            .WithMessage("Address must not exceed 256 characters.");

        RuleFor(x => x.Owners).NotNull();

        RuleForEach(x => x.Owners).SetValidator(new OwnerValidator());

        RuleFor(x => x.Accounts).NotNull();

        RuleForEach(x => x.Accounts).SetValidator(new AccountRequestValidator());
    }
}
