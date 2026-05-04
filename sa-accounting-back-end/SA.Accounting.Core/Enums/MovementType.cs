using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Enums;

public enum MovementType
{
    Deposit = 1,          // إضافة عهدة للموظف
    ApprovedExpense = 2,  // مصروفات معتمدة اتخصمت من العهدة
    Return = 3,           // الموظف رجّع فلوس
    AdjustmentIn = 4,     // تسوية بالزيادة
    AdjustmentOut = 5     // تسوية بالنقص
}
