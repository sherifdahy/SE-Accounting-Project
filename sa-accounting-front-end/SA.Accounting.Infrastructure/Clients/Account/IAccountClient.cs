using Refit;
using SA.Accounting.Core.Contracts.Profile.Requests;
using SA.Accounting.Core.Contracts.Profile.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Infrastructure.Clients.Account;

public interface IAccountClient
{
    [Get("/me")]
    Task<ProfileResponse> GetProfileAsync(CancellationToken cancellationToken = default);

    [Put("/me/info")]
    Task UpdateProfileAsync(UpdateProfileRequest request, CancellationToken cancellationToken = default);

    [Put("/me/change-password")]
    Task ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken = default);
}
