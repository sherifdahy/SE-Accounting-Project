using SA.Accounting.Application.Commands.Owner;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Interfaces;

namespace SA.Accounting.Application.Handlers.CommandsHandler.OwnerCommandsHandler;

public class ToggleStatusOwnerCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ToggleStatusOwnerCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result> Handle(ToggleStatusOwnerCommand request, CancellationToken cancellationToken)
    {
        var owner = await _unitOfWork.Owners.GetByIdAsync(request.Id,cancellationToken);

        if (owner == null)
            return Result.Failure(OwnerErrors.NotFound);

        owner.IsDeleted = !owner.IsDeleted;

        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }
}
