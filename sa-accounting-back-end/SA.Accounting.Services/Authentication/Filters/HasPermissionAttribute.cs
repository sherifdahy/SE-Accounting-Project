namespace SA.Accounting.Services.Authentication.Filters;
public class HasPermissionAttribute(string permission) : AuthorizeAttribute(permission)
{
}
