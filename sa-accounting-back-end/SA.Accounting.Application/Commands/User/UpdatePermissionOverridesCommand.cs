using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.User;

public record UpdatePermissionOverridesCommand(int UserId, List<string> DeniedPermissions) : IRequest<Result>;
