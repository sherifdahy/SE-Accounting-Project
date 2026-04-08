using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Owner.Responses;

public record OwnerResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SSN { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
}
