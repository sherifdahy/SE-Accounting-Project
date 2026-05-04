using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Errors;


public static class ExpenseCategoryErrors
{
    public static readonly Error NotFound = new(
        "ExpenseCategory.NotFound",
        "Expense category was not found.",
        StatusCodes.Status404NotFound);

    public static readonly Error DuplicateName = new(
        "ExpenseCategory.DuplicateName",
        "An expense category with the same name already exists.",
        StatusCodes.Status409Conflict);

    public static readonly Error InUse = new(
        "ExpenseCategory.InUse",
        "Cannot delete this category because it is used in expense claims. You can deactivate it instead.",
        StatusCodes.Status409Conflict);

    public static readonly Error AlreadyInactive = new(
        "ExpenseCategory.AlreadyInactive",
        "Expense category is already inactive.",
        StatusCodes.Status400BadRequest);
}