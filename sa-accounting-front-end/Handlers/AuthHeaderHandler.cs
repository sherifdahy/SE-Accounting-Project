using SA.Accounting.WPF.Abstraction;
using SA.Accounting.WPF.Contracts.Auth.Responses;
using SA.Accounting.WPF.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SA.Accounting.WPF.Handlers;

public class AuthHeaderHandler(ICacheService cacheService) : DelegatingHandler
{
    private readonly ICacheService _cacheService = cacheService;
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await _cacheService.GetAsync<AuthResponse>(KeysConstant.AuthResponse);
           
        if (response != null)
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",response.Token);

        return await base.SendAsync(request, cancellationToken);
    }
}
