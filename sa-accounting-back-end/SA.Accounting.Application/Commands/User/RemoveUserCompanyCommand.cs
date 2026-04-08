using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.User;

public record RemoveUserCompanyCommand : IRequest<Result>
{
    public int UserId { get; set; }
    public int CompanyId { get; set; }
}
