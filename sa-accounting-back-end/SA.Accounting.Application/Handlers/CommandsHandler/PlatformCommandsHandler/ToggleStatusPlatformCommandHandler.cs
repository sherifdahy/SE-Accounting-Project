using SA.Accounting.Application.Commands.Platform;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Handlers.CommandsHandler.PlatformCommandsHandler;

public class ToggleStatusPlatformCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ToggleStatusPlatformCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(ToggleStatusPlatformCommand request, CancellationToken cancellationToken)
    {
        var platform = await _unitOfWork.Platforms.GetByIdAsync(request.Id);

        if (platform == null) 
            return Result.Failure(PlatformErrors.NotFound);

        platform.IsDeleted = !platform.IsDeleted;

        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }
}
