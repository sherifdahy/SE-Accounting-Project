using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SA.Accounting.Core.Enums;

public enum TransactionState : byte
{
    [Description("جديد")]
    Initial = 0,

    [Description("مقبول")]
    Approved = 1,

    [Description("مرفوض")]
    Rejected = 2
}
