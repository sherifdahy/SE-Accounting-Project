using SA.Accounting.WPF.Abstraction;
using SA.Accounting.WPF.Contracts.Common;
using SA.Accounting.WPF.Contracts.Company.Requests;
using SA.Accounting.WPF.Contracts.Company.Responses;

namespace SA.Accounting.WPF.Interfaces;

public interface ICompanyService
{
    Task<PaginatedList<CompanyResponse>> GetAllAsync(RequestFilters filters);
    Task<CompanyResponse> CreateAsync(CreateCompanyRequest request);
    Task<CompanyDetailResponse> GetByIdAsync(int id);
    Task Update(int id, UpdateCompanyRequest request);
    Task ToggleStatusAsync(int id);
}
