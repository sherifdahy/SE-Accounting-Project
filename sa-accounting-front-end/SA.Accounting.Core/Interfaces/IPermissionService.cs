namespace SA.Accounting.Core.Interfaces;

public interface IPermissionService
{
    Task<bool> HasPermissionAsync(string permission);
    Task<string> GetUserEmailAsync();
}
