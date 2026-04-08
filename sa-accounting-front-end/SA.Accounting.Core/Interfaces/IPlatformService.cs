using SA.Accounting.Core.Contracts.Platform.Requests;
using SA.Accounting.Core.Contracts.Platform.Responses;

namespace SA.Accounting.Core.Interfaces;

public interface IPlatformService
{
    Task<List<PlatformResponse>> GetAllAsync(bool isDisabled = false);
    Task<PlatformDetailResponse> CreateAsync(PlatformRequest request);
    Task<PlatformDetailResponse> GetByIdAsync(int id);
    Task UpdateAsync(int id, PlatformRequest request);
    Task ToggleStatusAsync(int id);
}
