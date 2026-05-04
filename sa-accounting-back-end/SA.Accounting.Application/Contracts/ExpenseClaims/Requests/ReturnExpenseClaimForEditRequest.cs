using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.ExpenseClaims.Requests;
public class ReturnExpenseClaimForEditRequest
{
    public string Reason { get; set; } = string.Empty;
}
