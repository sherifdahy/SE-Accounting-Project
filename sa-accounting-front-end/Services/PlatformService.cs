using SA.Accounting.WPF.Clients.Platform;
using SA.Accounting.WPF.Contracts.Platform.Requests;
using SA.Accounting.WPF.Contracts.Platform.Responses;
using SA.Accounting.WPF.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.Services;

public class PlatfromService(IPlatformClient platformClient) : IPlatformService
{
    private readonly IPlatformClient _platformClient = platformClient;

    public async Task<PlatformDetailResponse> CreateAsync(PlatformRequest request)
    {
        return await _platformClient.CreateAsync(request);
    }

    public async Task<List<PlatformResponse>> GetAllAsync(bool isDisabled = false)
    {
        var platforms = await _platformClient.GetAllAsync(isDisabled);

        return platforms ?? [];
    }

    public async Task<PlatformDetailResponse> GetByIdAsync(int id)
    {
        return await _platformClient.GetAsync(id);
    }

    public async Task ToggleStatusAsync(int id)
    {
        await _platformClient.ToggleStatusAsync(id);
    }

    public async Task UpdateAsync(int id, PlatformRequest request)
    {
        await _platformClient.UpdateAsync(id, request);
    }
}
