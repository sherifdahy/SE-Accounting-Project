using SA.Accounting.Core.Entities.Relations;

namespace SA.Accounting.Core.Entities.Identity;

public class ApplicationUser : IdentityUser<int>
{
    public string Name { get; set; } = string.Empty;
    public string SSN { get; set; } = string.Empty;
    public bool IsDisabled { get; set; }
    public virtual ICollection<UserCompany> UserCompanies { get; set; } = new HashSet<UserCompany>();
    public ICollection<CompanyUserTransaction> CompanyUserTransaction { get; set; } = new HashSet<CompanyUserTransaction>();
}
