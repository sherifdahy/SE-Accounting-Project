using SA.Accounting.Core.Abstraction;
using SA.Accounting.Core.Contracts.Common;
using SA.Accounting.Core.Contracts.Company.Responses;
using SA.Accounting.Core.Contracts.User.Requests;
using SA.Accounting.Core.Contracts.User.Responses;

namespace SA.Accounting.Core.Interfaces;

public interface IUserService
{
    Task<PaginatedList<UserResponse>> GetUsersAsync(RequestFilters filters,bool includeDisabled);
    Task<UserResponse> GetUserByIdAsync(int id);
    Task CreateUserAsync(CreateUserRequest request);
    Task UpdateUserAsync(int id,UpdateUserRequest request);
    Task ToggleStatusAsync(int id);
    Task<List<CompanyResponse>> GetUserCompaniesAsync(int id, RequestFilters filters);
    Task AssignCompaniesToUserAsync(int id,AssignUserCompaniesRequest request);
    Task DeleteUserCompanyAsync(int userId, int companyId);
    Task DeleteUserCompaniesAsync(string userId);
}
