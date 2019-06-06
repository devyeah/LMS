using System;
using DevYeah.LMS.Business.ConfigurationModels;
using DevYeah.LMS.Business.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace DevYeah.LMS.Business
{
    public class EmailClient : IEmailClient
    {

        private readonly EmailSettings _emailSettings;
        public EmailClient(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public void SendEmail(string email, string subject, string content)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));
            if (string.IsNullOrWhiteSpace(subject)) throw new ArgumentNullException(nameof(subject));
            if (string.IsNullOrWhiteSpace(content)) throw new ArgumentNullException(nameof(content));

            var message = WrappingEmail(email, subject, content);
            var host = _emailSettings.Host;
            var port = _emailSettings.Port;
            var isUseSSL = _emailSettings.UseSsl;
            using (var client = new SmtpClient())
            {
                client.Connect(host, port, isUseSSL);
                client.Authenticate("name", "password");
                client.Send(message);
                client.Disconnect(true);
            }
        }

        private MimeMessage WrappingEmail(string email, string subject, string content)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.OfficialEmailAddress));
            message.To.Add(new MailboxAddress(email));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = content };
            return message;
        }
    }
}
