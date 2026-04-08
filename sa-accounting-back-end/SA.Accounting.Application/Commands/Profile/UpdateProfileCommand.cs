using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.User;

public record UpdateProfileCommand : IRequest<Result>
{
    public const string Route = "info";
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
