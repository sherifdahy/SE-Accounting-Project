using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Account.Requests;

public class UpdateAccountRequest
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int PlatformId { get; set; }
}
