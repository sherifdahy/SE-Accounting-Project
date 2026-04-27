namespace SA.Accounting.Core.Interfaces;

public interface IPermissionService
{
    Task<List<string>> GetAllAsync(CancellationToken cancellationToken = default);
}
