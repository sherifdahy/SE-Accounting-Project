using Refit;
using SA.Accounting.Core.Abstraction;
using SA.Accounting.Core.Contracts.Transaction.Requests;
using SA.Accounting.Core.Contracts.Transaction.Responses;
using SA.Accounting.Core.Contracts.TransactionCategory.Requests;
using SA.Accounting.Core.Contracts.TransactionCategory.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Infrastructure.Clients.TransactionCategory;

public interface ITransactionCategoryClient
{
    [Get("/api/transactionCategories")]
    Task<List<TransactionCategoryResponse>> GetAllAsync(bool includeDisabled, CancellationToken cancellationToken = default);

    [Get("/api/transactionCategories/{id}")]
    Task<TransactionCategoryResponse> GetAsync(int id, CancellationToken cancellationToken);

    [Post("/api/transactionCategories")]
    Task<TransactionCategoryResponse> CreateAsync(TransactionCategoryRequest request, CancellationToken cancellationToken);

    [Put("/api/transactionCategories/{id}")]
    Task UpdateAsync(int id, TransactionCategoryRequest request, CancellationToken cancellationToken);

    [Delete("/api/transactionCategories/{id}")]
    Task RemoveAsync(int id, CancellationToken cancellationToken);
}
