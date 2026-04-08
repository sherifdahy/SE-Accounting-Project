using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.Company;

public record ToggleStatusCompanyCommand : IRequest<Result>
{
    public int Id { get; set; }
};
