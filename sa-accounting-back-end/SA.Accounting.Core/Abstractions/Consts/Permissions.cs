namespace SA.Accounting.Core.Abstractions.Consts;

public static class Permissions
{

    public const string Type = "permissions";

    #region Companies Permissions
    public const string ReadCompanies = "companies:read";
    public const string CreateCompanies = "companies:create";
    public const string UpdateCompanies = "companies:update";
    public const string ToggleStatusCompanies = "companies:toggleStatus";
    #endregion

    #region Roles Permissions
    public const string GetRoles = "roles:read";
    public const string CreateRoles = "roles:create";
    public const string UpdateRoles = "roles:update";
    public const string ToggleStatusRoles = "roles:toggleStatus";
    #endregion

    #region Platforms Permissions
    public const string GetPlatforms = "platforms:read";
    public const string CreatePlatfroms = "platforms:create";
    public const string UpdatePlatfroms = "platforms:update";
    public const string ToggleStatusPlatfroms = "platforms:toggleStatus";
    #endregion

    #region Users Permissions
    public const string GetUsers = "users:read";
    public const string CreateUsers = "users:create";
    public const string UpdateUsers = "users:update";
    public const string ToggleStatusUsers = "users:toggleStatus";
    #endregion

    public static IList<string> GetAllPermissions()
    {
        return typeof(Permissions).GetFields().Select(x => x.GetValue(x) as string).ToList()!;
    }
}
