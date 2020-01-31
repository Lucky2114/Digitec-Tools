using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Threading.Tasks;

namespace Shopping_Tools.Source
{
    public static class EmailSender
    {
        public static async Task Send(string email, string message, string subject)
        {
            var gmailSecretsPath = Environment.GetEnvironmentVariable("DIGITEC_TOOLS_GMAIL_CREDENTIALS");
            if (string.IsNullOrEmpty(gmailSecretsPath))
            {
                throw new Exception("Path to Gmail Credentials not set. Set the environment variable DIGITEC_TOOLS_GMAIL_CREDENTIALS to a file that contains two lines: " +
                    "user=[value] \n" +
                    "password=[value] \n" +
                    "user is the email address.");
            }

            var gmailSecretsContents = File.ReadAllText(gmailSecretsPath);
            var user = gmailSecretsContents.Split(new string[] { "user=", "password=" }, StringSplitOptions.RemoveEmptyEntries)[0];
            var password = gmailSecretsContents.Split(new string[] { "user=", "password=" }, StringSplitOptions.RemoveEmptyEntries)[1];

            var fromAddress = new MailAddress(user, "Shopping Tools Notification");
            var toAddress = new MailAddress(email, "To Name");

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, password)
            };
            using var mailMessage = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = message
            };
            await smtp.SendMailAsync(mailMessage);
        }
    }
}
