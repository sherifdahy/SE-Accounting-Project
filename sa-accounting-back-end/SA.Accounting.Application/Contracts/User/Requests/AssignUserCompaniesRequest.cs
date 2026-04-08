using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.User.Requests;

public class AssignUserCompaniesRequest
{
    public List<int> CompanyIds { get; set; } = [];
}
