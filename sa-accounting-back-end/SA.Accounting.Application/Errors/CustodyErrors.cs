using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Errors;

public static class CustodyErrors
{
    public static readonly Error NotFound = new(
        "Custody.NotFound",
        "Custody was not found.",
        StatusCodes.Status404NotFound);

    public static readonly Error UserNotFound = new(
        "Custody.UserNotFound",
        "User was not found.",
        StatusCodes.Status404NotFound);

    public static readonly Error UserHasActiveCustody = new(
        "Custody.UserHasActiveCustody",
        "User already has an active custody. Close it before creating a new one.",
        StatusCodes.Status409Conflict);

    public static readonly Error NotActive = new(
        "Custody.NotActive",
        "Custody is not active.",
        StatusCodes.Status400BadRequest);

    public static readonly Error CannotCloseWithBalance = new(
        "Custody.CannotCloseWithBalance",
        "Cannot close custody while it has a non-zero balance. Settle or return the balance first.",
        StatusCodes.Status409Conflict);

    public static readonly Error InsufficientBalance = new(
        "Custody.InsufficientBalance",
        "Insufficient custody balance for this movement.",
        StatusCodes.Status400BadRequest);

    public static readonly Error InvalidMovementType = new(
        "Custody.InvalidMovementType",
        "ApprovedExpense movements cannot be created manually. They are produced by claim settlement.",
        StatusCodes.Status400BadRequest);
}
