using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.Account;

public record ToggleStatusAccountCommand : IRequest<Result>
{
    public int CompanyId { get; set; }
    public int PlatformId { get; set; }
}
