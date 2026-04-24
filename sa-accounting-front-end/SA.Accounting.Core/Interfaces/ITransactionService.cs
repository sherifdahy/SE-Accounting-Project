using SA.Accounting.Core.Abstraction;
using SA.Accounting.Core.Contracts.Common;
using SA.Accounting.Core.Contracts.Platform.Requests;
using SA.Accounting.Core.Contracts.Platform.Responses;
using SA.Accounting.Core.Contracts.Transaction.Requests;
using SA.Accounting.Core.Contracts.Transaction.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Interfaces;

public interface ITransactionService
{
    Task<PaginatedList<TransactionResponse>> GetAllAsync(RequestFilters filters,CancellationToken cancellationToken = default);
    Task<PaginatedList<TransactionResponse>> GetAllAsync(int userId,RequestFilters filters, CancellationToken cancellationToken = default);
    Task<TransactionDetailResponse> GetByIdAsync(int id,CancellationToken cancellationToken = default);
    Task<TransactionDetailResponse> CreateAsync(int userId,CreateTransactionRequest request,CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, UpdateTransactionRequest request,CancellationToken cancellationToken = default);
    Task ChangeState(int id,ChangeTransactionStateRequest request,CancellationToken cancellationToken = default);
    Task RemoveAsync(int id,CancellationToken cancellationToken = default);
}
