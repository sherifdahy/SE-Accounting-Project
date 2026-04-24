using SA.Accounting.Core.Entities.Base;
using SA.Accounting.Core.Entities.Identity;
using SA.Accounting.Core.Enums;

namespace SA.Accounting.Core.Entities.Transactions;

public class Transaction : AuditableEntity
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
    public string Note { get; set; } = string.Empty;
    public TransactionState CurrentState { get; set; }
    public int UserId { get; set; }
    public virtual ApplicationUser User { get; set; } = default!;
    public virtual ICollection<TransactionItem> Items { get; set; } = new HashSet<TransactionItem>();
    public virtual ICollection<TransactionHistory> Histories { get; set; } = new HashSet<TransactionHistory>();
}
