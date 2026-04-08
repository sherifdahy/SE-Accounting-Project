using Mapster;
using SA.Accounting.Application.Commands.Selector;
using SA.Accounting.Application.Contracts.Selector.Responses;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Interfaces;
using SA.Accounting.Core.Entities.Platforms;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Handlers.CommandsHandler.SelectorCommandsHandler;

public class CreateSelectorCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateSelectorCommand, Result<SelectorResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<SelectorResponse>> Handle(CreateSelectorCommand request, CancellationToken cancellationToken)
    {
        if (!_unitOfWork.Platforms.IsExist(x => x.Id == request.PlatformId))
            return Result.Failure<SelectorResponse>(PlatformErrors.NotFound);

        var selector = request.Adapt<Selector>();

        await _unitOfWork.Selectors.AddAsync(selector);
        await _unitOfWork.SaveAsync();

        return Result.Success(selector.Adapt<SelectorResponse>());
    }
}
