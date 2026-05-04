using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SA.Accounting.Core.Enums;

public enum ExpenseClaimState
{
    Draft = 1,             // الموظف لسه بيكتب
    Submitted = 2,         // الموظف بعت للمراجعة
    UnderReview = 3,       // المحاسب بيراجع
    Approved = 4,          // كل البنود اتوافقت
    PartiallyApproved = 5, // بعض البنود اتوافقت وبعضها اترفض
    Rejected = 6,          // كله اترفض
    Settled = 7,           // اتخصم من العهدة
    Cancelled = 8,         // اتلغى
    ReturnedForEdit = 9    // رجع للموظف يعدل
}