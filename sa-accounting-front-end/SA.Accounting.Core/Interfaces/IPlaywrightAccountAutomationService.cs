using SA.Accounting.Core.Contracts.Account.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Interfaces;

public interface IAccountAutomationService
{
    Task OpenAsync(AccountResponse account,CancellationToken cancellationToken = default);
    Task CloseAsync(CancellationToken cancellationToken = default);
}
