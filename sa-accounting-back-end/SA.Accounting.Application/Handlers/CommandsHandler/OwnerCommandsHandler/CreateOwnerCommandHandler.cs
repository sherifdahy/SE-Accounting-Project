using Mapster;
using SA.Accounting.Application.Commands.Owner;
using SA.Accounting.Application.Contracts.Owner.Responses;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Companies;
using SA.Accounting.Core.Entities.Interfaces;

namespace SA.Accounting.Application.Handlers.CommandsHandler.OwnerCommandsHandler;

public class CreateOwnerCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateOwnerCommand, Result<OwnerResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<OwnerResponse>> Handle(CreateOwnerCommand request, CancellationToken cancellationToken)
    {
        if (!_unitOfWork.Companies.IsExist(x => x.Id == request.CompanyId))
            return Result.Failure<OwnerResponse>(CompanyErrors.NotFound);

        var oData = request.Adapt<Owner>();
       
        await _unitOfWork.Owners.AddAsync(oData, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success(oData.Adapt<OwnerResponse>());
    }
}
