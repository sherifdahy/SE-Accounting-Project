using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Contracts.Auth.Responses;

public class CurrentUserResponse
{
    public int UserId { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; } = [];
    public List<string> Permissions { get; set; } = [];
}
