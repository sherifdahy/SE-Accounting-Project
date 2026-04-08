using MediaFoundation.MFPlayer;
using SA.Accounting.WPF.Abstraction;
using SA.Accounting.WPF.Clients.Company;
using SA.Accounting.WPF.Contracts.Common;
using SA.Accounting.WPF.Contracts.Company.Requests;
using SA.Accounting.WPF.Contracts.Company.Responses;
using SA.Accounting.WPF.Interfaces;

namespace SA.Accounting.WPF.Services;

public class CompanyService(ICompanyClient client) : ICompanyService
{
    private readonly ICompanyClient _client = client;

    public async Task<CompanyResponse> CreateAsync(CreateCompanyRequest request)
    {
        return await _client.CreateAsync(request);
    }
    public async Task<PaginatedList<CompanyResponse>> GetAllAsync(RequestFilters filters)
    {
        return await _client.GetAllAsync(filters);
    }
    public async Task<CompanyDetailResponse> GetByIdAsync(int id)
    {
        return await _client.GetAsync(id);
    }
    public async Task Update(int id, UpdateCompanyRequest request)
    {
        await _client.UpdateAsync(id, request);
    }
    public async Task ToggleStatusAsync(int id)
    {
        await _client.ToggleStatusAsync(id);
    }
}
