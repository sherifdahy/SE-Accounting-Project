using SA.Accounting.Core.Entities.Companies;
using SA.Accounting.Core.Entities.Platforms;
using SA.Accounting.Core.Entities.Relations;
using SA.Accounting.Core.Entities.Transactions;
using SA.Accounting.Core.Interfaces;

namespace SA.Accounting.Core.Entities.Interfaces;
public interface IUnitOfWork : IDisposable
{
    public IRepository<Company> Companies { get; }
    public IRepository<Owner> Owners { get; }
    public IRepository<Platform> Platforms { get; }
    public IRepository<Selector> Selectors { get; }
    public IRepository<Transaction> Transactions { get; }
    public IRepository<TransactionCategory> TransactionCategories { get; }
    public IRepository<TransactionItem> TransactionItems { get; }
    public IUserCompaniesRepository UserCompanies { get; }
    public IRepository<Account> Accounts { get; }
    int Save();
    Task<int> SaveAsync(CancellationToken cancellationToken = default);
}
