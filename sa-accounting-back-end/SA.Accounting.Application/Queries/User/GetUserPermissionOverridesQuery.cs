using SA.Accounting.Application.Contracts.PermissionOverride.Responses;

namespace SA.Accounting.Application.Queries.User;

public record GetUserPermissionOverridesQuery(int UserId) : IRequest<Result<UserPermissionOverridesResponse>>;
