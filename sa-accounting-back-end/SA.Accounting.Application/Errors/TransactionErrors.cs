using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Errors;

public static class TransactionErrors
{
    public static Error NotFound => new("Transaction.Notfound", "Transaction is not Exists", StatusCodes.Status404NotFound);
    public static Error CannotEditApprovedTransaction => new("Transaction.CannotEditApprovedTransaction", "Cannot Edit Approved Transaction", StatusCodes.Status400BadRequest);
    public static Error InvalidStateTransition => new("Transaction.InvalidStateTransition", "Invalid State Transition", StatusCodes.Status400BadRequest);
    public static Error RejectReasonRequired => new("Transaction.RejectReasonRequired", "Reject Reason Required", StatusCodes.Status400BadRequest);

}
