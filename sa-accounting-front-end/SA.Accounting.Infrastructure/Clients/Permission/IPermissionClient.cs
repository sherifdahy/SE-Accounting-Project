using Refit;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Infrastructure.Clients.Permission;

public interface IPermissionClient
{
    [Get("/api/permissions")]
    Task<List<string>> GetAllAsync(CancellationToken cancellationToken = default);
}
