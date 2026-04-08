using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.Auth;

public record ConfirmEmailCommand : IRequest<Result>
{
    public const string Route = "/auth/confirmEmail";
    public string UserId { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}
