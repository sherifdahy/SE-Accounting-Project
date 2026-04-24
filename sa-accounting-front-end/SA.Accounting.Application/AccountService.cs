using SA.Accounting.Core.Contracts.Profile.Requests;
using SA.Accounting.Core.Contracts.Profile.Responses;
using SA.Accounting.Core.Interfaces;
using SA.Accounting.Infrastructure.Clients.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Services;

public class AccountService(IAccountClient client) : IAccountService
{
    private readonly IAccountClient _client = client;
    public async Task ChangePasswordAsync(ChangePasswordRequest reqeust, CancellationToken cancellationToken = default)
    {
        await _client.ChangePasswordAsync(reqeust, cancellationToken);
    }

    public async Task<ProfileResponse> GetProfileAsync(CancellationToken cancellationToken = default)
    {
        return await _client.GetProfileAsync(cancellationToken);
    }

    public async Task UpdateProfileAsync(UpdateProfileRequest request, CancellationToken cancellationToken = default)
    {
        await _client.UpdateProfileAsync(request, cancellationToken);
    }
}
