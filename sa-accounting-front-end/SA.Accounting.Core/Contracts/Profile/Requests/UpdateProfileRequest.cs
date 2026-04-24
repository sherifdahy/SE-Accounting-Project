using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Contracts.Profile.Requests;

public record UpdateProfileRequest 
{
    public string Name { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string SSN { get; set; } = string.Empty;
}
