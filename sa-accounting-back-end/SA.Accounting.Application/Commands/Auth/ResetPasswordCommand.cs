using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.Auth;

public record ResetPasswordCommand : IRequest<Result>
{
    public const string Route = "reset-password";
    public string Email { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
