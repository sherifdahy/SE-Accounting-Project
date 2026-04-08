using Refit;
using SA.Accounting.WPF.Abstraction;
using SA.Accounting.WPF.Contracts.Common;
using SA.Accounting.WPF.Contracts.Company.Requests;
using SA.Accounting.WPF.Contracts.Company.Responses;

namespace SA.Accounting.WPF.Clients.Company;

public interface ICompanyClient
{
    [Get("/api/companies")]
    Task<PaginatedList<CompanyResponse>> GetAllAsync(RequestFilters requestFilters, bool IncludeDisabled = false);
    
    [Get("/api/companies/{id}")]
    Task<CompanyDetailResponse> GetAsync(int id);

    [Post("/api/companies")]
    Task<CompanyResponse> CreateAsync([Body] CreateCompanyRequest request);

    [Put("/api/companies/{id}")]
    Task UpdateAsync(int id,[Body] UpdateCompanyRequest request);

    [Delete("/api/companies/{id}")]
    Task ToggleStatusAsync(int id);
}
