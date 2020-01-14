using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace Digitec_Tools_Web.Services
{
    public class EmailSender : IEmailSender
    {
        readonly string SendGirdUser;
        readonly string SendGridKey;

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            SendGirdUser = Environment.GetEnvironmentVariable("SendGridUser");
            SendGridKey = Environment.GetEnvironmentVariable("SendGridKey");
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(SendGridKey, subject, message, email);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("noreply@pleasedont.com", SendGirdUser),
                Subject = "Digitec Tools Account Verification",
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}