using SA.Accounting.Application.Contracts.Platform.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Account.Responses;

partial class AccountDetailResponse
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public PlatformResponse Platform { get; set; } = default!;
}
