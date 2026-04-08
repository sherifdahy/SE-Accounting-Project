using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.User;

public record AssignUserCompaniesCommand : IRequest<Result>
{
    public int UserId { get; set; }
    public List<int> CompanyIds { get; set; } = [];
}
