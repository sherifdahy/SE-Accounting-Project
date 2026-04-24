using SA.Accounting.Core.Contracts.TransactionCategory.Requests;
using SA.Accounting.Core.Contracts.TransactionCategory.Responses;
using SA.Accounting.Core.Interfaces;
using SA.Accounting.Infrastructure.Clients.TransactionCategory;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Services;

public class TransactionCategoryService(ITransactionCategoryClient client) : ITransactionCategoryService
{
    private readonly ITransactionCategoryClient _client = client;

    public async Task<TransactionCategoryResponse> CreateAsync(TransactionCategoryRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.CreateAsync(request, cancellationToken);
    }

    public async Task<List<TransactionCategoryResponse>> GetAllAsync(bool includeDisabled, CancellationToken cancellationToken = default)
    {
        return await _client.GetAllAsync(includeDisabled, cancellationToken);
    }

    public async Task<TransactionCategoryResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _client.GetAsync(id, cancellationToken);
    }

    public async Task RemoveAsync(int id, CancellationToken cancellationToken = default)
    {
         await _client.RemoveAsync(id, cancellationToken);
    }

    public async Task UpdateAsync(int id, TransactionCategoryRequest request, CancellationToken cancellationToken = default)
    {
        await _client.UpdateAsync(id, request, cancellationToken);
    }
}
