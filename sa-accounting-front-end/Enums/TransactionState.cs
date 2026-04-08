using System.ComponentModel;

namespace SA.Accountring.WPF.Enums;

public enum TransactionState : byte
{
    [Description("جديد")]
    Initial = 0,

    [Description("مقبول")]
    Approved = 1,

    [Description("مرفوض")]
    Rejected = 2
}
