namespace SA.Accounting.Core.Abstractions.Consts;

public class DefaultRoles
{
    public const string Admin = nameof(Admin);
    public const string AdminRoleConcurrencyStamp = "727F7C04-0A04-4012-9AA0-5BDF83C53788";
    public const int AdminRoleId = 1;

    public const string Member = nameof(Member);
    public const string MemberRoleConcurrencyStamp = "10208DE5-8AD0-41E3-BFED-CFD49C46BEDF";
    public const int MemberRoleId = 2;

    public const string Employee = nameof(Employee);
    public const string EmployeeRoleConcurrencyStamp = "D5CD1328-D599-4608-B5B0-C00056B6E7D7";
    public const int EmployeeRoleId = 3;
}
