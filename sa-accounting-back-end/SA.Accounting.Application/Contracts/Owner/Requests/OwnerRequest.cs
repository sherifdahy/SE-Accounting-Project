using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Owner.Requests;

public record OwnerRequest
{
    public string Name { get; set; } = string.Empty;
    public string SSN { get; set; } = string.Empty;
}
