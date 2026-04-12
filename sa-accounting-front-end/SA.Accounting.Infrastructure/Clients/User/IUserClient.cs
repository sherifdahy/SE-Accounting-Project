using Refit;
using SA.Accounting.Core.Abstraction;
using SA.Accounting.Core.Contracts.Common;
using SA.Accounting.Core.Contracts.Company.Responses;
using SA.Accounting.Core.Contracts.User.Requests;
using SA.Accounting.Core.Contracts.User.Responses;

namespace SA.Accounting.Infrastructure.Clients.User;

public interface IUserClient
{
    [Get("/api/users")]
    public Task<PaginatedList<UserResponse>> GetUsersAsync([Body]RequestFilters filters,[Query]bool includeDisabled);

    [Get("/api/users/{id}")]
    public Task<UserResponse> GetUserByIdAsync(int id);

    [Post("/api/users")]
    public Task CreateUserAsync([Body]CreateUserRequest request);

    [Put("/api/users/{id}")]
    public Task UpdateUserAsync(int id, [Body]UpdateUserRequest request);

    [Patch("/api/user/{id}/toggle-status")]
    public Task ToggleStatusAsync(int id);

    [Get("/api/users/{id}/companies")]
    public Task<List<CompanyResponse>> GetUserCompaniesAsync(int id,RequestFilters filters);

    [Post("/api/users/{id}/companies")]
    public Task AssignCompaniesToUserAsync(int id, [Body] AssignUserCompaniesRequest request);

    [Delete("/api/users/{userId}/companies/{companyId}")]
    public Task DeleteUserCompanyAsync(int userId, int companyId);

    [Delete("/api/users/{id}")]
    public Task DeleteUserCompaniesAsync(string id);
}
