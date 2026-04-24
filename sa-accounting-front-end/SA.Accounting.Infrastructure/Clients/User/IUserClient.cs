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
    public Task<PaginatedList<UserResponse>> GetAllAsync([Body]RequestFilters filters,[Query]bool includeDisabled);

    [Get("/api/users/{id}")]
    public Task<UserResponse> GetByIdAsync(int id);

    [Post("/api/users")]
    public Task CreateAsync([Body]CreateUserRequest request);

    [Put("/api/users/{id}")]
    public Task UpdateAsync(int id, [Body]UpdateUserRequest request);

    [Patch("/api/user/{id}/toggle-status")]
    public Task ToggleStatusAsync(int id);

    [Get("/api/users/{userId}/companies")]
    Task<PaginatedList<CompanyResponse>> GetUserCompaniesAsync(int userId, [Query] RequestFilters filters);

    [Post("/api/users/{userId}/companies/{companyId}")]
    Task AssignCompanyToUserAsync(int userId, int companyId);

    [Delete("/api/users/{userId}/companies/{companyId}")]
    Task RemoveCompanyFromUserAsync(int userId, int companyId);

    [Post("/api/users/{userId}/companies/all")]
    Task AssignAllCompaniesToUserAsync(int userId);

    [Delete("/api/users/{userId}/companies/all")]
    Task RemoveAllCompaniesFromUserAsync(int userId);
}
