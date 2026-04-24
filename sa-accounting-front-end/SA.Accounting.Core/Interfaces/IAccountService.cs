using SA.Accounting.Core.Contracts.Profile.Requests;
using SA.Accounting.Core.Contracts.Profile.Responses;

namespace SA.Accounting.Core.Interfaces;

public interface IAccountService 
{
    Task<ProfileResponse> GetProfileAsync(CancellationToken cancellationToken = default);
    Task UpdateProfileAsync(UpdateProfileRequest request,CancellationToken cancellationToken = default);
    Task ChangePasswordAsync(ChangePasswordRequest reqeust, CancellationToken cancellationToken = default);
}
