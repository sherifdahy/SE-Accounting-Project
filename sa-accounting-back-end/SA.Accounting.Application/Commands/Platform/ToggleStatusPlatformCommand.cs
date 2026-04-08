using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.Platform;

public record ToggleStatusPlatformCommand : IRequest<Result>
{
    public int Id { get; set; }
}
