using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Interfaces;

public interface IExpenseClaimNumberGenerator
{
    Task<string> GenerateAsync(CancellationToken cancellationToken = default);
}
