using SA.Accounting.Core.Abstraction;
using SA.Accounting.Core.Contracts.Common;
using SA.Accounting.Core.Contracts.Company.Responses;
using SA.Accounting.Core.Contracts.User.Requests;
using SA.Accounting.Core.Contracts.User.Responses;
using SA.Accounting.Core.Interfaces;
using SA.Accounting.Infrastructure.Clients.User;

namespace SA.Accounting.Services;

public class UserService(IUserClient client) : IUserService
{
    private readonly IUserClient _client = client;
    public async Task AssignCompaniesToUserAsync(int id, AssignUserCompaniesRequest request)
    {
        await _client.AssignCompaniesToUserAsync(id, request);
    }

    public async Task CreateUserAsync(CreateUserRequest request)
    {
        await _client.CreateUserAsync(request);
    }

    public async Task DeleteUserCompaniesAsync(string userId)
    {
        await _client.DeleteUserCompaniesAsync(userId);
    }

    public async Task DeleteUserCompanyAsync(int userId, int companyId)
    {
        await _client.DeleteUserCompanyAsync(userId, companyId);
    }

    public async Task<UserResponse> GetUserByIdAsync(int id)
    {
        return await _client.GetUserByIdAsync(id);
    }

    public async Task<List<CompanyResponse>> GetUserCompaniesAsync(int id, RequestFilters filters)
    {
        return await _client.GetUserCompaniesAsync(id, filters);
    }

    public async Task<PaginatedList<UserResponse>> GetUsersAsync(RequestFilters filters, bool includeDisabled)
    {
        return await _client.GetUsersAsync(filters, includeDisabled);
    }

    public async Task ToggleStatusAsync(int id)
    {
        await _client.ToggleStatusAsync(id);
    }

    public async Task UpdateUserAsync(int id, UpdateUserRequest request)
    {
        await _client.UpdateUserAsync(id, request);
    }
}
