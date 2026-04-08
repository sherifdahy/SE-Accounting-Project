using Mapster;
using SA.Accounting.Application.Commands.Platform;
using SA.Accounting.Application.Contracts.Platform.Responses;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Interfaces;
using SA.Accounting.Core.Entities.Platforms;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Handlers.CommandsHandler.PlatformCommandsHandler;

public class CreatePlatformCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreatePlatformCommand, Result<PlatformDetailResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<PlatformDetailResponse>> Handle(CreatePlatformCommand request, CancellationToken cancellationToken)
    {
        if (_unitOfWork.Platforms.IsExist(x => x.Name == request.Name))
            return Result.Failure<PlatformDetailResponse>(PlatformErrors.DuplicatedName);

        var platform = request.Adapt<Platform>();

        await _unitOfWork.Platforms.AddAsync(platform);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success(platform.Adapt<PlatformDetailResponse>());
    }
}
