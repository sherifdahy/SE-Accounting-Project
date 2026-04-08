using Mapster;
using SA.Accounting.Application.Commands.Owner;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Companies;
using SA.Accounting.Core.Entities.Interfaces;

namespace SA.Accounting.Application.Handlers.CommandsHandler.OwnerCommandsHandler;

public class UpdateOwnerCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateOwnerCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result> Handle(UpdateOwnerCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Owners.GetByIdAsync(request.Id) is not Owner owner)
            return Result.Failure(CompanyErrors.NotFound);

        request.Adapt(owner);

        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }
}
