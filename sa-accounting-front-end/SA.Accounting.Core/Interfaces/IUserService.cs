using SA.Accounting.Core.Abstraction;
using SA.Accounting.Core.Contracts.Common;
using SA.Accounting.Core.Contracts.Company.Responses;
using SA.Accounting.Core.Contracts.User.Requests;
using SA.Accounting.Core.Contracts.User.Responses;

namespace SA.Accounting.Core.Interfaces;

public interface IUserService
{
    Task<PaginatedList<UserResponse>> GetAllAsync(RequestFilters filters,bool includeDisabled=false);
    Task<UserResponse> GetByIdAsync(int id);
    Task CreateAsync(CreateUserRequest request);
    Task UpdateAsync(int id,UpdateUserRequest request);
    Task ToggleStatusAsync(int id);
    Task<PaginatedList<CompanyResponse>> GetUserCompaniesAsync(int userId,RequestFilters filters);
    Task AssignCompanyToUserAsync(int userId, int companyId);
    Task RemoveCompanyFromUserAsync(int userId, int companyId);
    Task AssignAllCompaniesToUserAsync(int userId);
    Task RemoveAllCompaniesFromUserAsync(int userId);
}
