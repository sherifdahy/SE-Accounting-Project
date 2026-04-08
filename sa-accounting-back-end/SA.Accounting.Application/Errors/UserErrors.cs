using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SA.Accounting.Application.Errors;
public static class UserErrors
{
    public static readonly Error InvalidCredentials
        = new ("User.InvalidCredentials", "Invalid Email / Password.", StatusCodes.Status401Unauthorized);

    public static readonly Error UserNotAllowed
        = new ("User.UserNotAllowed", "UserNotAllowed.", StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidJwtToken
        = new ("User.InvalidJwtToken", "Invalid Jwt Token.", StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidRefreshToken
        = new ("User.InvalidRefreshToken", "Invalid Refresh Token.", StatusCodes.Status401Unauthorized);

    public static readonly Error DuplicatedEmail
        = new("User.DuplicatedEmail", "Another User with the same email is already exists.", StatusCodes.Status409Conflict);

    public static readonly Error NotFound
        = new("User.NotFound", Description: "User is not Found.", StatusCodes.Status404NotFound);

    public static readonly Error DuplicateSSN
    = new("User.SSN", Description: "Another User with the same SSN is already exists.", StatusCodes.Status409Conflict);

    public static readonly Error EmailNotConfirmed
        = new ("User.EmailNotConfirmed", "Email Not Confirmed", StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidCode
        = new ("User.InvalidCode", "Invalid Code", StatusCodes.Status401Unauthorized);

    public static readonly Error DuplicatedConfirmation
        = new ("User.DuplicatedConfirmation", "Email is Already Confirmed", StatusCodes.Status400BadRequest);

    public static readonly Error LockedUser
        = new ("Auth.LockedUser", "Locked User please contact administrator.", StatusCodes.Status401Unauthorized);

}
