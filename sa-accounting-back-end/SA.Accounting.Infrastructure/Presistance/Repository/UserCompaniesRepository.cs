using SA.Accounting.Core.Interfaces;
using SA.Accounting.Infrastructure.Repository;

namespace SA.Accounting.Infrastructure.Presistance.Repository;

public class UserCompaniesRepository : Repository<UserCompany>, IUserCompaniesRepository
{
    public UserCompaniesRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task AssignAllCompaniesToUserAsync(int userId,CancellationToken cancellationToken)
    {
        await _context.UserCompanies.Where(x => x.UserId == userId).ExecuteDeleteAsync(cancellationToken);

        var companies = await _context.Companies
            .Where(x => !x.IsDeleted)
            .Select(c => new UserCompany
            {
                CompanyId = c.Id,
                UserId = userId
            })
            .ToListAsync();

        await _context.UserCompanies.AddRangeAsync(companies);
    }

    public async Task RemoveAllCompaniesFromUserAsync(int userId,CancellationToken cancellationToken)
    {
        await _context.UserCompanies.Where(x => x.UserId == userId).ExecuteDeleteAsync(cancellationToken);
    }
}
