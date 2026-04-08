using SA.Accounting.Core.Entities.Base;
using SA.Accounting.Core.Entities.Platforms;

namespace SA.Accounting.Core.Entities.Companies;

public class Account : AuditableEntity
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public int CompanyId { get; set; }
    public Company Company { get; set; } = default!;

    public int PlatformId { get; set; }
    public Platform Platform { get; set; } = default!;
}
