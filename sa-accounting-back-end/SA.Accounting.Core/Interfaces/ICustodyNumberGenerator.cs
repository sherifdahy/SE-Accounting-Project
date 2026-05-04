using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Interfaces;

public interface ICustodyNumberGenerator
{
    Task<string> GenerateAsync(CancellationToken cancellationToken = default);
}
