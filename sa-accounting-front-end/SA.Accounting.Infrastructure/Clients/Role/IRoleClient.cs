using Refit;
using SA.Accounting.Core.Contracts.Role.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Infrastructure.Clients.Role;

public interface IRoleClient
{
    [Get("/api/roles")]
    Task<List<RoleResponse>> GetAllAsync(bool includeDisabled = false);
}
