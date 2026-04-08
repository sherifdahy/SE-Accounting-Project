using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SA.Accounting.Core.Interfaces;

namespace SA.Accounting.Services;

public class CacheService(IDistributedCache distributedCache) : ICacheService
{
    private readonly IDistributedCache _distributedCache = distributedCache;
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        var result = await _distributedCache.GetStringAsync(key, cancellationToken);

        if (string.IsNullOrEmpty(result))
            return null;

        return JsonConvert.DeserializeObject<T>(result);
    }
    public async Task SetAsync<T>(string key, T value,TimeSpan? absoluteExpire = null, CancellationToken cancellationToken = default) where T : class
    {
        var option = new DistributedCacheEntryOptions();

        if (absoluteExpire.HasValue)
            option.AbsoluteExpirationRelativeToNow = absoluteExpire;

        await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(value), option, cancellationToken);
    }
    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);
    }

    
}

