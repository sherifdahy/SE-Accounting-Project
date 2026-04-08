using Refit;
using SA.Accounting.WPF.Abstraction;
using SA.Accounting.WPF.Contracts.Company.Requests;
using SA.Accounting.WPF.Contracts.Company.Responses;
using SA.Accounting.WPF.Contracts.Platform.Requests;
using SA.Accounting.WPF.Contracts.Platform.Responses;
using SA.Accounting.WPF.Portals.SystemAdministration.UserControls.Platforms;

namespace SA.Accounting.WPF.Clients.Platform;

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
