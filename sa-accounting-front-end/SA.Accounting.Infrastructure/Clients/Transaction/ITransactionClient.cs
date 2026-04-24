using Refit;
using SA.Accounting.Core.Abstraction;
using SA.Accounting.Core.Contracts.Common;
using SA.Accounting.Core.Contracts.Transaction.Requests;
using SA.Accounting.Core.Contracts.Transaction.Responses;

namespace SA.Accounting.Infrastructure.Clients.Transaction;

public interface ITransactionClient
{
    [Get("/api/transactions")]
    Task<PaginatedList<TransactionResponse>> GetAllAsync(RequestFilters filters, CancellationToken cancellationToken = default);

    [Get("/api/transactions/users/{userId}")]
    Task<PaginatedList<TransactionResponse>> GetAllAsync(int userId,RequestFilters filters, CancellationToken cancellationToken = default);

    [Get("/api/transactions/{id}")]
    Task<TransactionDetailResponse> GetAsync(int id, CancellationToken cancellationToken);

    [Post("/api/transactions/users/{userId}")]
    Task<TransactionDetailResponse> CreateAsync(int userId,CreateTransactionRequest request ,CancellationToken cancellationToken);

    [Put("/api/transactions/{id}")]
    Task UpdateAsync(int id,UpdateTransactionRequest request, CancellationToken cancellationToken);

    [Patch("/api/transactions/{id}")]
    Task ChangeStateAsync(int id, ChangeTransactionStateRequest request, CancellationToken cancellationToken);

    [Delete("/api/transactions/{id}")]
    Task RemoveAsync(int id, CancellationToken cancellationToken);

}
