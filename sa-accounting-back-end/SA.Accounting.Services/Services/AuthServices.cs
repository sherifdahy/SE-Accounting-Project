using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using SA.Accounting.Core.Entities.Identity;
using SA.Accounting.Core.Interfaces;
using SA.Accounting.Services.Helpers;
using System.Text;

namespace SA.Accounting.Services.Services;

public class AuthServices : IAuthServices
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailSender _emailSender;
    private IHttpContextAccessor _httpContextAccessor;
    public AuthServices(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IEmailSender emailSender)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _emailSender = emailSender;
    }
    public Task GetTokenAsync(string email, string password)
    {
        throw new NotImplementedException();
    }
    public async Task<IdentityResult> ResetPasswordAsync(ApplicationUser user,string code , string newPassword)
    {
        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            return await _userManager.ResetPasswordAsync(user!, code,newPassword);
        }
        catch (FormatException)
        {
            return IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
        }
    }
    public async Task SendResetPasswordEmail(ApplicationUser applicationUser)
    {
        var code = await _userManager.GeneratePasswordResetTokenAsync(applicationUser);

        // code encryption
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        // send email
        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

        var emailBody = EmailBodyBuilder.GenerateEmailBody("ForgetPassword", new Dictionary<string, string>()
                {
                    { "{{name}}" , applicationUser.Name },
                    { "{{action_url}}" , $"{origin}/auth/forgetPassword?email={applicationUser.Email}&code={code}" }
                });

        BackgroundJob.Enqueue(() =>
            _emailSender.SendEmailAsync(applicationUser.Email!, "SA Accounting Reset Password", emailBody)
        );
    }
    public async Task SendConfirmationEmail(ApplicationUser applicationUser)
    {
        var code = await _userManager.GeneratePasswordResetTokenAsync(applicationUser);

        // code encryption
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        // send email
        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

        var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation", new Dictionary<string, string>()
                {
                    { "{{name}}" , applicationUser.Name },
                    { "{{action_url}}" , $"{origin}/auth/emailConfiguration?userId={applicationUser.Id}&code={code}" }
                });

        BackgroundJob.Enqueue(() =>
            _emailSender.SendEmailAsync(applicationUser.Email!, "SA Accounting Reset Password", emailBody)
        );
    }
}
