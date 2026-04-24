using Microsoft.AspNetCore.Identity;
using SA.Accounting.Application.Commands.User;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Identity;
using SA.Accounting.Core.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Handlers.CommandsHandler.UserCommandsHandler;

public class AssignAllCompaniesToUserCommandHandler(IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager) : IRequestHandler<AssignAllCompaniesToUserCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<Result> Handle(AssignAllCompaniesToUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userManager.FindByIdAsync(request.UserId.ToString()) is not ApplicationUser user)
            return Result.Failure(UserErrors.NotFound);

        await _unitOfWork.UserCompanies.AssignAllCompaniesToUserAsync(request.UserId, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }
}
