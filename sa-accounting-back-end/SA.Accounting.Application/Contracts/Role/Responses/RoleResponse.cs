using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Role.Responses;

public class RoleResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
}
