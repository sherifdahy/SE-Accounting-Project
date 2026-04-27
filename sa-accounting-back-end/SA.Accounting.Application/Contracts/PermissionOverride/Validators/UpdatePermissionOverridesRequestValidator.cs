using SA.Accounting.Application.Contracts.PermissionOverride.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Permission.Validators;

public class UpdatePermissionOverridesRequestValidator : AbstractValidator<UpdateUserPermissionOverridesRequest>
{
    public UpdatePermissionOverridesRequestValidator()
    {
        RuleFor(x => x.DeniedPermissions)
            .Must(x => x.Distinct().Count() == x.Count)
            .WithMessage("Permission must be unique.");
    }
}
