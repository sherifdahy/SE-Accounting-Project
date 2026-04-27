using SA.Accounting.Application.Contracts.Role.Requests;
using SA.Accounting.Core.Abstractions.Consts;

namespace SA.Accounting.Application.Contracts.Role.Validators;

public class RoleRequestValidator : AbstractValidator<RoleRequest>
{
    public RoleRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Permissions)
            .NotNull()
            .NotEmpty()
            .Must(x=> x.Distinct().Count() == x.Count)
            .WithMessage("Permissions must be unique");
    }
}