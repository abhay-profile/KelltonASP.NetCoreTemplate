using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using TwoFactorAuthentication.Utilities;

namespace TwoFactorAuthentication.Services;

public class EmailSender : IEmailSender
{
    private readonly EmailConfiguration? _emailConfiguration;
    private readonly ILogger<EmailSender> _logger;
    public EmailSender(IConfiguration _configuration, ILogger<EmailSender> logger)
    {
        _emailConfiguration = _configuration.GetSection("EmailConfiguration")?.Get<EmailConfiguration>();
        _logger = logger;
    }
    public Task SendEmailAsync(string receiversEmail, string subject, string htmlMessage)
    {
        if (_emailConfiguration is null || string.IsNullOrEmpty(_emailConfiguration.SendersEmail) || string.IsNullOrEmpty(_emailConfiguration.SendGridApiKey))
        {
            _logger.LogError("Unable to get email configuration section from appsetting.json file.");
            return Task.CompletedTask;
        }else
        {
            var client = new SendGridClient(_emailConfiguration.SendGridApiKey);

            var from = new EmailAddress(_emailConfiguration.SendersEmail, subject);
            var to = new EmailAddress(receiversEmail);
            var message = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);

            return client.SendEmailAsync(message);
        }
    }
}
