using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
namespace SA.Accounting.Services.Services;

public class EmailServices : IEmailSender
{
    private readonly MailSettings _mailSettings;
    public EmailServices(IOptions<MailSettings> options)
    {
        _mailSettings = options.Value;
    }
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MimeMessage()
        {
            Sender = MailboxAddress.Parse(_mailSettings.Mail),
            Subject = subject,
        };

        message.To.Add(MailboxAddress.Parse(email));

        var builder = new BodyBuilder()
        {
            HtmlBody = htmlMessage
        };

        message.Body = builder.ToMessageBody();

        using var stmp = new SmtpClient();

        stmp.ServerCertificateValidationCallback = (s, c, h, e) => true;
        
        await stmp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);

        stmp.Authenticate(_mailSettings.Mail, _mailSettings.Password);

        await stmp.SendAsync(message);

        stmp.Disconnect(true);
    }
}
