using SA.Accounting.Core.Entities.Base;

namespace SA.Accounting.Core.Entities.Transactions;

public class TransactionCategory : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public virtual ICollection<TransactionItem> TransactionItems { get; set; } = new HashSet<TransactionItem>();
}
