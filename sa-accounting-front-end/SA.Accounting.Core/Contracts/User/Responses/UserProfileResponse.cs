using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Contracts.User.Responses;

public class UserProfileResponse
{
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}
