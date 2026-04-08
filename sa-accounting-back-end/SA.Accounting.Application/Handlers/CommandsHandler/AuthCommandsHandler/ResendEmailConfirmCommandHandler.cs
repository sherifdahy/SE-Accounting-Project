using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SA.Accounting.Application.Commands.Auth;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Identity;
using SA.Accounting.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Handlers.CommandsHandler.AuthCommandsHandler;

public class ResendEmailConfirmCommandHandler : IRequestHandler<ResendConfirmEmailCommand, Result>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<ResendEmailConfirmCommandHandler> _logger;
    private readonly IEmailSender _emailSender;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ResendEmailConfirmCommandHandler(UserManager<ApplicationUser> userManager, ILogger<ResendEmailConfirmCommandHandler> logger, IEmailSender emailSender, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _logger = logger;
        _emailSender = emailSender;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<Result> Handle(ResendConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
            return Result.Success();

        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.DuplicatedConfirmation);

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        // code encryption
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        // send email
        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

        var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation", new Dictionary<string, string>()
                {
                    { "{{name}}" , user.Name },
                    { "{{action_url}}" , $"{origin}/auth/emailConfiguration?userId={user.Id}&code={code}" }
                });

        BackgroundJob.Enqueue(() =>
            _emailSender.SendEmailAsync(user.Email!, "SA Accounting", emailBody)
        );
        
        return Result.Success();
    }
}
