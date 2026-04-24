using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.User;

public record RemoveCompanyFromUserCommand : IRequest<Result>
{
    public int UserId { get; set; }
    public int CompanyId { get; set; }
}
