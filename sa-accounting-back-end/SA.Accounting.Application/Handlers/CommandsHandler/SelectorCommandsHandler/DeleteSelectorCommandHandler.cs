using SA.Accounting.Application.Commands.Selector;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Handlers.CommandsHandler.SelectorCommandsHandler;

public class DeleteSelectorCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteSelectorCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(DeleteSelectorCommand request, CancellationToken cancellationToken)
    {
        var selector = await _unitOfWork.Selectors.GetByIdAsync(request.Id);

        if (selector == null) 
            return Result.Failure(SelectorErrors.NotFound);
        
        _unitOfWork.Selectors.Delete(selector);
        _unitOfWork.Save();

        return Result.Success();
    }
}
