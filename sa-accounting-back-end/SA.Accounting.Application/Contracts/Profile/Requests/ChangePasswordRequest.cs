using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Profile.Requests;

public record ChangePasswordRequest 
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
