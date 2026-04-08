using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.User;

public record ToggleStatusUserCommand : IRequest<Result>
{
    public int UserId { get; set; }
}
