using SA.Accounting.Core.Abstraction;
using SA.Accounting.Core.Contracts.Common;
using SA.Accounting.Core.Contracts.Transaction.Requests;
using SA.Accounting.Core.Contracts.Transaction.Responses;
using SA.Accounting.Core.Interfaces;
using SA.Accounting.Infrastructure.Clients.Transaction;

namespace SA.Accounting.Services;

public class TransactionService(ITransactionClient client) : ITransactionService
{
    private readonly ITransactionClient _client = client;

    public async Task ChangeState(int id, ChangeTransactionStateRequest request, CancellationToken cancellationToken = default)
    {
        await _client.ChangeStateAsync(id,request, cancellationToken);
    }

    public async Task<TransactionDetailResponse> CreateAsync(int userId, CreateTransactionRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.CreateAsync(userId, request, cancellationToken);
    }

    public async Task<PaginatedList<TransactionResponse>> GetAllAsync(RequestFilters filters, CancellationToken cancellationToken = default)
    {
        return await _client.GetAllAsync(filters, cancellationToken);
    }

    public async Task<PaginatedList<TransactionResponse>> GetAllAsync(int userId, RequestFilters filters, CancellationToken cancellationToken = default)
    {
        return await _client.GetAllAsync(userId,filters, cancellationToken);
    }

    public async Task<TransactionDetailResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _client.GetAsync(id, cancellationToken);
    }

    public async Task RemoveAsync(int id, CancellationToken cancellationToken = default)
    {
        await _client.RemoveAsync(id,cancellationToken);
    }

    public async Task UpdateAsync(int id, UpdateTransactionRequest request, CancellationToken cancellationToken = default)
    {
        await _client.UpdateAsync(id, request, cancellationToken);
    }
}
