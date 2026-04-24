using SA.Accounting.Core.Contracts.Role.Responses;
using SA.Accounting.Core.Interfaces;
using SA.Accounting.Infrastructure.Clients.Role;

namespace SA.Accounting.Services;

public class RoleService : IRoleService
{
    private readonly IRoleClient _roleClient;
    public RoleService(IRoleClient roleClient)
    {
        _roleClient = roleClient;
    }
    public Task<List<RoleResponse>> GetAllAsync(bool includeDisabled = false)
    {
        return _roleClient.GetAllAsync(includeDisabled);
    }
}
