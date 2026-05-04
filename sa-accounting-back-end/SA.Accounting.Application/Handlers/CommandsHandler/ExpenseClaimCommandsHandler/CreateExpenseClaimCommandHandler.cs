using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SA.Accounting.Application.Commands.ExpenseClaim;
using SA.Accounting.Application.Contracts.ExpenseClaims.Responses;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.ExpenseClaims;
using SA.Accounting.Core.Entities.Identity;
using SA.Accounting.Core.Entities.Interfaces;
using SA.Accounting.Core.Enums;
using SA.Accounting.Core.Interfaces;

namespace SA.Accounting.Application.Handlers.CommandsHandler.ExpenseClaimCommandsHandler;

public class CreateExpenseClaimHandler(UserManager<ApplicationUser> userManager,IUnitOfWork unitOfWork,IExpenseClaimNumberGenerator expenseClaimNumberGenerator) : IRequestHandler<CreateExpenseClaimCommand, Result<ExpenseClaimResponse>>
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IExpenseClaimNumberGenerator _expenseClaimNumberGenerator = expenseClaimNumberGenerator;
    public async Task<Result<ExpenseClaimResponse>> Handle(CreateExpenseClaimCommand command,CancellationToken cancellationToken)
    {
        var request = command.Request;

        // 1. Validate user
        var user =  _userManager.Users
            .FirstOrDefault(x => x.Id == command.UserId);

        if (user is null)
            return Result.Failure<ExpenseClaimResponse>(ExpenseClaimErrors.UserNotFound);

        // 2. Validate companies
        var companyIds = request.Items.Select(x => x.CompanyId).Distinct().ToList();
        var existingCompanies = (await _unitOfWork.Companies
            .FindAllAsync(x => companyIds.Contains(x.Id), [],cancellationToken))
            .Select(x => x.Id)
            .ToList();

        if (existingCompanies.Count != companyIds.Count)
            return Result.Failure<ExpenseClaimResponse>(ExpenseClaimErrors.CompanyNotFound);

        // 3. Validate categories
        var categoryIds = request.Items.Select(x => x.ExpenseCategoryId).Distinct().ToList();
        var categories = await _unitOfWork.ExpenseCategories
            .FindAllAsync(x => categoryIds.Contains(x.Id), [],cancellationToken);

        if (categories.Count() != categoryIds.Count)
            return Result.Failure<ExpenseClaimResponse>(ExpenseClaimErrors.CategoryNotFound);

        if (categories.Any(c => c.IsDisabled))
            return Result.Failure<ExpenseClaimResponse>(ExpenseClaimErrors.InactiveCategory);

        // 4. Cross-table validation: RequiresAttachment
        var categoryDict = categories.ToDictionary(c => c.Id);
        foreach (var item in request.Items)
        {
            var category = categoryDict[item.ExpenseCategoryId];
            if (category.RequiresAttachment && string.IsNullOrWhiteSpace(item.FileUrl))
                return Result.Failure<ExpenseClaimResponse>(ExpenseClaimErrors.AttachmentRequired);
        }

        // 5. Build the entity
        var claim = new ExpenseClaim
        {
            Number = await _expenseClaimNumberGenerator.GenerateAsync(cancellationToken),
            ClaimDate = request.ClaimDate,
            Note = request.Note ?? string.Empty,
            CurrentState = ExpenseClaimState.Draft,
            UserId = command.UserId,
            Items = request.Items.Select(i => new ExpenseClaimItem
            {
                CompanyId = i.CompanyId,
                ExpenseCategoryId = i.ExpenseCategoryId,
                Note = i.Note,
                FileUrl = i.FileUrl,
                Amount = i.Amount,
                State = ExpenseClaimItemState.Pending
            }).ToList()
        };

        await _unitOfWork.ExpenseClaims.AddAsync(claim, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        // 6. Reload with navigation properties for response
        var savedClaim = await _unitOfWork.ExpenseClaims.FindAsync(x => x.Id == claim.Id,
            [
                x=>x.Include(x=>x.User),
                x=>x.Include(x=>x.Items).ThenInclude(i=>i.Company),
                x=>x.Include(x=>x.Items).ThenInclude(i=>i.ExpenseCategory),
                x=>x.Include(x=>x.Histories).ThenInclude(i=>i.CreatedBy),

            ]
            , cancellationToken);

        return Result.Success(savedClaim.Adapt<ExpenseClaimResponse>());
    }
}
