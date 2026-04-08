using SA.Accounting.Application.Contracts.Platform.Responses;
using SA.Accounting.Application.Contracts.Selector.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Account.Responses;

public record AccountResponse
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public PlatformResponse Platform { get; set; } = default!;
}
