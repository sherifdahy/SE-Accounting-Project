using SA.Accounting.Core.Abstraction;
using SA.Accounting.Infrastructure.Clients.Company;
using SA.Accounting.Core.Contracts.Common;
using SA.Accounting.Core.Contracts.Company.Requests;
using SA.Accounting.Core.Contracts.Company.Responses;
using SA.Accounting.Core.Interfaces;

namespace SA.Accounting.Services;

public class CompanyService(ICompanyClient client) : ICompanyService
{
    private readonly ICompanyClient _client = client;

    public async Task<CompanyResponse> CreateAsync(CreateCompanyRequest request)
    {
        return await _client.CreateAsync(request);
    }
    public async Task<PaginatedList<CompanyResponse>> GetAllAsync(RequestFilters filters, bool IncludeDisabled = false)
    {
        return await _client.GetAllAsync(filters,IncludeDisabled);
    }
    public async Task<CompanyDetailResponse> GetByIdAsync(int id)
    {
        return await _client.GetAsync(id);
    }
    public async Task UpdateAsync(int id, UpdateCompanyRequest request)
    {
        await _client.UpdateAsync(id, request);
    }
    public async Task ToggleStatusAsync(int id)
    {
        await _client.ToggleStatusAsync(id);
    }
}

