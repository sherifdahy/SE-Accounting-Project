using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SA.Accounting.Application.Contracts.User.Responses;
using SA.Accounting.Application.Queries.User;
using SA.Accounting.Core.Abstractions;
using SA.Accounting.Core.Entities.Identity;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace SA.Accounting.Application.Handlers.QueriesHandler.UserQueriesHandler;

public class GetAllUserQueryHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<GetAllUsersQuery, Result<PaginatedList<UserResponse>>>
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<Result<PaginatedList<UserResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var searchValue = request.Filters.SearchValue;

        Expression<Func<ApplicationUser, bool>> filter = x =>
            (string.IsNullOrEmpty(request.Filters.SearchValue) ||
             x.Email!.Contains(request.Filters.SearchValue) ||
             x.SSN.Contains(request.Filters.SearchValue))
            &&
            (request.IncludeDisabled == true ? true : x.IsDisabled == false);

        var query = _userManager.Users.Where(filter);

        var count = await _userManager.Users.CountAsync(filter);

        if (!string.IsNullOrEmpty(request.Filters.SortColumn))
            query = query.OrderBy(request.Filters.SortColumn);

        var applicationUsers = await query
            .Skip(request.Filters.PageSize * (request.Filters.PageNumber - 1))
            .Take(request.Filters.PageSize)
            .ToListAsync(cancellationToken);

        var users = new List<UserResponse>();

        foreach (var appUser in applicationUsers)
        {
            var userResponse = appUser.Adapt<UserResponse>(); 
            var roles = await _userManager.GetRolesAsync(appUser);
            userResponse.Role = roles.First();
            users.Add(userResponse);
        }

        return Result.Success(PaginatedList<UserResponse>.Create(users, count, request.Filters.PageNumber, request.Filters.PageSize));
    }
}
