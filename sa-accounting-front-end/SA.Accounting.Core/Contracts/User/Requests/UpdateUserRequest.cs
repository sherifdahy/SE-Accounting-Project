using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Contracts.User.Requests;

public class UpdateUserRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Password { get; set; }
    public string SSN { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
