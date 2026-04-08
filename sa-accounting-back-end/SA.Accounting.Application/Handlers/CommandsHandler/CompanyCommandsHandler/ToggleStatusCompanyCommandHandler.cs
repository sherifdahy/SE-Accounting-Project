using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Interfaces;

namespace SA.Accounting.Application.Handlers.CommandsHandler.CompanyCommandsHandler;

public class ToggleStatusCompanyCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ToggleStatusCompanyCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result> Handle(ToggleStatusCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await _unitOfWork.Companies.GetByIdAsync(request.Id,cancellationToken);

        if (company == null)
            return Result.Failure(CompanyErrors.NotFound);

        company.IsDeleted = !company.IsDeleted;

        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }
}
