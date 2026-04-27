using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Role.Responses;

public record RoleDetailResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<string> Permissions { get; set; } = [];
}
