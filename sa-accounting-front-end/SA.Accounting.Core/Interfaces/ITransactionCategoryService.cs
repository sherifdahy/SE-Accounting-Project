using SA.Accounting.Core.Abstraction;
using SA.Accounting.Core.Contracts.Common;
using SA.Accounting.Core.Contracts.Transaction.Requests;
using SA.Accounting.Core.Contracts.Transaction.Responses;
using SA.Accounting.Core.Contracts.TransactionCategory.Requests;
using SA.Accounting.Core.Contracts.TransactionCategory.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Interfaces;

public interface ITransactionCategoryService
{
    Task<List<TransactionCategoryResponse>> GetAllAsync(bool includeDisabled, CancellationToken cancellationToken = default);
    Task<TransactionCategoryResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TransactionCategoryResponse> CreateAsync(TransactionCategoryRequest request, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, TransactionCategoryRequest request, CancellationToken cancellationToken = default);
    Task RemoveAsync(int id, CancellationToken cancellationToken = default);
}
