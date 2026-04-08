using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.Interfaces;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key,CancellationToken cancellationToken = default) where T : class;
    Task SetAsync<T>(string key, T value,TimeSpan? absoluteExpire, CancellationToken cancellationToken = default) where T : class;
    Task RemoveAsync(string key,CancellationToken cancellationToken = default);
}
