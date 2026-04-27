using SA.Accounting.Infrastructure.Repository;
using SA.Accounting.Core.Entities.Companies;
using SA.Accounting.Core.Entities.Platforms;
using SA.Accounting.Core.Entities.Transactions;
using SA.Accounting.Core.Interfaces;
using SA.Accounting.Core.Entities.Identity;

namespace SA.Accounting.Infrastructure.Presistance.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;

        Companies = new Repository<Company>(_context);
        Owners = new Repository<Owner>(_context);
        Platforms = new Repository<Platform>(_context);
        Selectors = new Repository<Selector>(_context);
        Transactions = new Repository<Transaction>(_context);
        TransactionCategories = new Repository<TransactionCategory>(_context);
        TransactionItems = new Repository<TransactionItem>(_context);
        UserCompanies = new UserCompaniesRepository(_context);
        Accounts = new Repository<Account>(_context);
        DeniedPermissions = new Repository<UserRolePermissionOverride>(_context);
    }

    public IRepository<Company> Companies { get; }
    public IRepository<Owner> Owners { get; }
    public IRepository<Platform> Platforms { get; }
    public IRepository<Selector> Selectors { get; }
    public IRepository<Transaction> Transactions { get; }
    public IRepository<TransactionCategory> TransactionCategories { get; }
    public IRepository<TransactionItem> TransactionItems { get; }
    public IUserCompaniesRepository UserCompanies { get; }
    public IRepository<Account> Accounts { get; }
    public IRepository<UserRolePermissionOverride> DeniedPermissions { get; }
    public void Dispose()
    {
        _context.Dispose();
    }

    public int Save()
    {
        return _context.SaveChanges(); 
    }
    public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
