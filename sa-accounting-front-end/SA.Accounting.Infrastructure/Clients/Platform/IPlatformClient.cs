using Refit;
using SA.Accounting.Core.Abstraction;
using SA.Accounting.Core.Contracts.Company.Requests;
using SA.Accounting.Core.Contracts.Company.Responses;
using SA.Accounting.Core.Contracts.Platform.Requests;
using SA.Accounting.Core.Contracts.Platform.Responses;

namespace SA.Accounting.Infrastructure.Clients.Platform;

public interface IPlatformClient
{
    [Get("/api/platforms")]
    Task<List<PlatformResponse>?> GetAllAsync(bool includeDisabled = false);

    [Get("/api/platforms/{id}")]
    Task<PlatformDetailResponse> GetAsync(int id);

    [Post("/api/platforms")]
    Task<PlatformDetailResponse> CreateAsync([Body]  PlatformRequest request);

    [Put("/api/platforms/{id}")]
    Task UpdateAsync(int id, [Body] PlatformRequest request);

    [Delete("/api/platforms/{id}")]
    Task ToggleStatusAsync(int id);
}


