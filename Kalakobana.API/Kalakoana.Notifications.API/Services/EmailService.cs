using Kalakobana.Notifications.API.Infrastructure.Configurations;
using Kalakobana.Notifications.API.Infrastructure.HTMLs;
using Kalakobana.SharedKernel.Models;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace Kalakoana.Notifications.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly GoogleAppSettings _googleAppSettings;
        private const string _emailAddress = "mosquitobuzz@gmail.com";
        private readonly ILogger<EmailService> _logger;
        public EmailService(IOptions<GoogleAppSettings> googleAppSettings, ILogger<EmailService> logger)
        {
            _googleAppSettings = googleAppSettings.Value;
            _logger = logger;
        }

        public async Task SendEmail(MessageContractEvent ev, CancellationToken token)
        {
            try
            {
                string googleAppPassword = _googleAppSettings.GoogleAppPassword;
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(_emailAddress);
                mailMessage.To.Add(ev.SendTo);
                mailMessage.Subject = ev.Subject;
                if (ev.ActionType == MessageType.Confirmation.ToString())
                    mailMessage.Body = ConfirmationHtml.GenerateBody(ev);
                if (ev.ActionType == MessageType.PasswordReset.ToString())
                    mailMessage.Body = PasswordResetHTML.GenerateBody(ev);
                mailMessage.IsBodyHtml = true;
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential("mosqutiobuzz@gmail.com", googleAppPassword);
                await smtpClient.SendMailAsync(mailMessage, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email to {ev.SendTo}");
            }
        }
    }
}

