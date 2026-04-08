using SA.Accounting.Core.Abstraction;
using SA.Accounting.Core.Contracts.Common;
using SA.Accounting.Core.Contracts.Company.Requests;
using SA.Accounting.Core.Contracts.Company.Responses;

namespace SA.Accounting.Core.Interfaces;

public interface ICompanyService
{
    Task<PaginatedList<CompanyResponse>> GetAllAsync(RequestFilters filters);
    Task<CompanyResponse> CreateAsync(CreateCompanyRequest request);
    Task<CompanyDetailResponse> GetByIdAsync(int id);
    Task UpdateAsync(int id, UpdateCompanyRequest request);
    Task ToggleStatusAsync(int id);
}
