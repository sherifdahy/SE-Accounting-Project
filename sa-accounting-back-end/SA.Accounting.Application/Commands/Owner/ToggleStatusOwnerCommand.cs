using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.Owner;

public record ToggleStatusOwnerCommand : IRequest<Result>
{
    public int Id { get; set; }
};
