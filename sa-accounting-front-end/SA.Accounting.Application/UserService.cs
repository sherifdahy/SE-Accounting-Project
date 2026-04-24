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

    public async Task CreateAsync(CreateUserRequest request)
    {
        await _client.CreateAsync(request);
    }

    public async Task<UserResponse> GetByIdAsync(int id)
    {
        return await _client.GetByIdAsync(id);
    }
    public async Task<PaginatedList<UserResponse>> GetAllAsync(RequestFilters filters, bool includeDisabled)
    {
        return await _client.GetAllAsync(filters, includeDisabled);
    }

    public async Task ToggleStatusAsync(int id)
    {
        await _client.ToggleStatusAsync(id);
    }

    public async Task UpdateAsync(int id, UpdateUserRequest request)
    {
        await _client.UpdateAsync(id, request);
    }

    public async Task<PaginatedList<CompanyResponse>> GetUserCompaniesAsync(int userId, RequestFilters filters)
    {
        return await _client.GetUserCompaniesAsync(userId, filters);
    }

    public async Task AssignCompanyToUserAsync(int userId, int companyId)
    {
        await _client.AssignCompanyToUserAsync(userId, companyId);
    }

    public async Task RemoveCompanyFromUserAsync(int userId, int companyId)
    {
        await _client.RemoveCompanyFromUserAsync(userId, companyId);
    }

    public async Task AssignAllCompaniesToUserAsync(int userId)
    {
        await _client.AssignAllCompaniesToUserAsync(userId);
    }

    public async Task RemoveAllCompaniesFromUserAsync(int userId)
    {
        await _client.RemoveAllCompaniesFromUserAsync(userId);

    }
}
