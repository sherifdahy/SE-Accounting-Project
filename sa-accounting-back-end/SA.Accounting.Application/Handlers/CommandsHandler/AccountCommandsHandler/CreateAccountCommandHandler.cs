using Mapster;
using SA.Accounting.Application.Commands.Account;
using SA.Accounting.Application.Contracts.Account.Responses;
using SA.Accounting.Application.Contracts.Platform.Responses;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Companies;
using SA.Accounting.Core.Entities.Interfaces;
using SA.Accounting.Core.Entities.Platforms;
using SA.Accounting.Core.Entities.Relations;

namespace SA.Accounting.Application.Handlers.CommandsHandler.AccountCommandsHandler;

public class CreateAccountCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateAccountCommand, Result<AccountResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<AccountResponse>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        if (!_unitOfWork.Companies.IsExist(x => x.Id == request.CompanyId))
            return Result.Failure<AccountResponse>(CompanyErrors.NotFound);

        if (await _unitOfWork.Platforms.GetByIdAsync(request.PlatformId,cancellationToken) is not Platform platform)
            return Result.Failure<AccountResponse>(PlatformErrors.NotFound);

        if (_unitOfWork.Accounts.IsExist(x => x.CompanyId == request.CompanyId && x.PlatformId == request.PlatformId))
            return Result.Failure<AccountResponse>(AccountErrors.DuplicatedAccount);

        var account = request.Adapt<Account>();

        await _unitOfWork.Accounts.AddAsync(account, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        var response = account.Adapt<AccountResponse>();

        return Result.Success(response);
    }
}
