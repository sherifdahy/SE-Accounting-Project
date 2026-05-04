using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SA.Accounting.Application.Commands.Custody;
using SA.Accounting.Application.Contracts.Custodies.Responses;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Custodies;
using SA.Accounting.Core.Entities.Identity;
using SA.Accounting.Core.Entities.Interfaces;
using SA.Accounting.Core.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SA.Accounting.Application.Handlers.CommandsHandler.CustodyCommandsHandler;

public class CreateCustodyHandler(IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager,ICustodyNumberGenerator custodyNumberGenerator) : IRequestHandler<CreateCustodyCommand, Result<CustodyDetailsResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ICustodyNumberGenerator _custodyNumberGenerator = custodyNumberGenerator;

    public async Task<Result<CustodyDetailsResponse>> Handle(CreateCustodyCommand command,CancellationToken cancellationToken)
    {
        var userId = command.Request.UserId;

        var user = await _userManager.Users
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        if (user is null)
            return Result.Failure<CustodyDetailsResponse>(CustodyErrors.UserNotFound);

        var hasActive = _unitOfWork.Custodies
            .IsExist(x => x.UserId == userId && x.IsActive);

        if (hasActive)
            return Result.Failure<CustodyDetailsResponse>(CustodyErrors.UserHasActiveCustody);

        var custody = command.Request.Adapt<Custody>();

        custody.Number = await _custodyNumberGenerator.GenerateAsync(cancellationToken);

        await _unitOfWork.Custodies.AddAsync(custody, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        var response = custody.Adapt<CustodyDetailsResponse>();

        return Result.Success(response);
    }
}
