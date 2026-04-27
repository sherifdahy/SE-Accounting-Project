using SA.Accounting.Core.Abstraction;
using SA.Accounting.Core.Contracts.Auth.Responses;
using SA.Accounting.Core.Interfaces;
using SA.Accounting.Infrastructure.Clients.Permission;

namespace SA.Accounting.Services;

public class PermissionService(IPermissionClient client) : IPermissionService
{
    private readonly IPermissionClient _client = client;
    public async Task<List<string>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _client.GetAllAsync(cancellationToken);
    }
}

