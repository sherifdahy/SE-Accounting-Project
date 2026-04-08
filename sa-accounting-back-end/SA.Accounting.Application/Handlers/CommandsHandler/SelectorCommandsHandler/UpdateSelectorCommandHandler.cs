using Mapster;
using SA.Accounting.Application.Commands.Selector;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Interfaces;

namespace SA.Accounting.Application.Handlers.CommandsHandler.SelectorCommandsHandler;

public class UpdateSelectorCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateSelectorCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(UpdateSelectorCommand request, CancellationToken cancellationToken)
    {
        var selector = await _unitOfWork.Selectors.GetByIdAsync(request.Id);

        if (selector == null)
            return Result.Failure(SelectorErrors.NotFound);

        request.Adapt(selector);

        await _unitOfWork.SaveAsync();

        return Result.Success();
    }
}
