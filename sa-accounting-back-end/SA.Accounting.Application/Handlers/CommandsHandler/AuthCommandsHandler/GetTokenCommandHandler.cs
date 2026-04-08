using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SA.Accounting.Application.Commands.Auth;
using SA.Accounting.Application.Contracts.Auth.Responses;
using SA.Accounting.Application.Errors;
using SA.Accounting.Services.Authentication;
using SA.Accounting.Core.Entities.Identity;
using SA.Accounting.Core.Interfaces;

namespace SA.Accounting.Application.Handlers.CommandsHandler.AuthCommandsHandler;

public class GetTokenCommandHandler : IRequestHandler<GetTokenCommand, Result<AuthResponse>>
{

    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJWTProvider _jwtProvider;
    private readonly JwtOptions _options;

    public GetTokenCommandHandler(IOptions<JwtOptions> options, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, IJWTProvider provider)
    {
        _signInManager = signInManager;
        _roleManager = roleManager;
        _userManager = userManager;
        _jwtProvider = provider;
        _options = options.Value;
    }
    public async Task<Result<AuthResponse>> Handle(GetTokenCommand request, CancellationToken cancellationToken)
    {
        var applicationUser = await _userManager.FindByEmailAsync(request.Email);

        if (applicationUser is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var signInResult = await _signInManager.PasswordSignInAsync(applicationUser, request.Password, false, true);

        if (signInResult.Succeeded)
        {
            var roles = await _userManager.GetRolesAsync(applicationUser);

            var permissions = new List<string>();

            foreach (var roleName in roles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);

                if (role is null)
                    continue;

                var rolePermissions = await _roleManager.GetClaimsAsync(role);

                permissions.AddRange(rolePermissions.Select(x => x.Value).Distinct());
            }

            return Result.Success(new AuthResponse()
            {
                ExpiresIn = (_options.ExpiryMinutes * 60),
                Token = _jwtProvider.GeneratedToken(applicationUser, roles, permissions).token,
            });
        }

        return signInResult.IsNotAllowed
            ? Result.Failure<AuthResponse>(UserErrors.EmailNotConfirmed)
            : signInResult.IsLockedOut
            ? Result.Failure<AuthResponse>(UserErrors.LockedUser)
            : Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);
    }
}

