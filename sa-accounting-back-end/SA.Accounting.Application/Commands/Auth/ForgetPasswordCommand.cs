using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.Auth;

public record ForgetPasswordCommand : IRequest<Result>
{
    public const string Route = "forget-password";
    public string Email { get; set; } = string.Empty;
}
