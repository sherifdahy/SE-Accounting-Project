using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SA.Accounting.Application.Commands.User;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Identity;
using SA.Accounting.Core.Entities.Interfaces;
using SA.Accounting.Core.Entities.Relations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Handlers.CommandsHandler.UserCommandsHandler;

public class AssignUserCompaniesCommandHandler(IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager): IRequestHandler<AssignUserCompaniesCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    public async Task<Result> Handle(AssignUserCompaniesCommand request, CancellationToken cancellationToken)
    {
        if (!await _userManager.Users.AnyAsync(x => x.Id == request.UserId, cancellationToken))
            return Result.Failure(UserErrors.NotFound);

        foreach (var companyId in request.CompanyIds)
        {
            if (!_unitOfWork.Companies.IsExist(x => x.Id == companyId))
                continue;

            if (_unitOfWork.UserCompanies.IsExist(x => x.CompanyId == companyId && x.UserId == request.UserId))
                continue;

            var userCompany = new UserCompany
            {
                UserId = request.UserId,
                CompanyId = companyId
            };

            await _unitOfWork.UserCompanies.AddAsync(userCompany, cancellationToken);
        }

        await _unitOfWork.SaveAsync(cancellationToken);
        return Result.Success();
    }
}