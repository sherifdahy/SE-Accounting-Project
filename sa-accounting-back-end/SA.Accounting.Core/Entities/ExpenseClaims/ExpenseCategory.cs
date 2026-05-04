using SA.Accounting.Core.Entities.Base;

namespace SA.Accounting.Core.Entities.ExpenseClaims;

public class ExpenseCategory : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool RequiresAttachment { get; set; }
    public bool IsDisabled { get; set; }
    public virtual ICollection<ExpenseClaimItem> ExpenseClaimItems { get; set; } = new HashSet<ExpenseClaimItem>();
}
