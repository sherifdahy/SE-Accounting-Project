using SA.Accounting.Core.Contracts.Role.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Interfaces;

public interface IRoleService
{
    Task<List<RoleResponse>> GetAllAsync(bool includeDisabled = false);
}
