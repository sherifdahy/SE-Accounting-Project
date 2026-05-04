using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SA.Accounting.Core.Entities.Identity;
using SA.Accounting.Core.Interfaces;

namespace SA.Accounting.Services.Services;

public class UserServices : IUserServices
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserServices(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<ApplicationUser> GetProfileAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _userManager.Users.Where(x=>x.Id == userId).SingleAsync(cancellationToken);
    }

    public async Task<IdentityResult> UpdateProfileAsync(ApplicationUser applicationUser,
        CancellationToken cancellationToken = default)
    {
        return await _userManager.UpdateAsync(applicationUser);
    }

    public async Task<IdentityResult> ChangePasswordAsync(int userId,
        string currentPassword,
        string newPassword,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        return await _userManager.ChangePasswordAsync(user!, currentPassword, newPassword);
    }
}
