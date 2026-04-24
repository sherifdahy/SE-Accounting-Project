using SA.Accounting.Core.Entities.Interfaces;
using SA.Accounting.Core.Entities.Relations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Interfaces;

public interface IUserCompaniesRepository : IRepository<UserCompany>
{
    Task AssignAllCompaniesToUserAsync(int userId, CancellationToken cancellationToken);
    Task RemoveAllCompaniesFromUserAsync(int userId, CancellationToken cancellationToken);
}
