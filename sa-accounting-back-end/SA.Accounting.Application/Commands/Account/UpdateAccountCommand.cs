using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.Account;

public record UpdateAccountCommand : IRequest<Result>
{
    public int CompanyId { get; set; }
    public int PlatfromId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
