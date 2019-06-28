using System;
using System.Threading;
using DevYeah.LMS.Business.ConfigurationModels;
using DevYeah.LMS.Business.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace DevYeah.LMS.Business
{
    public class EmailClient : IEmailClient
    {

        private readonly AppSettings _appSettings;
        public EmailClient(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public void SendEmail(string email, string subject, string content)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));
            if (string.IsNullOrWhiteSpace(subject)) throw new ArgumentNullException(nameof(subject));
            if (string.IsNullOrWhiteSpace(content)) throw new ArgumentNullException(nameof(content));

            var message = CreateEmailMessage(email, subject, content);
            RetryAction(() => SendEmail(message), _appSettings.MaxRetryCount);
        }

        private void SendEmail(MimeMessage message)
        {
            var host = _appSettings.EmailConfig.Host;
            var port = _appSettings.EmailConfig.Port;
            var isUseSSL = _appSettings.EmailConfig.UseSsl;
            var accountName = _appSettings.EmailConfig.AccountName;
            var password = _appSettings.EmailConfig.Password;
            using (var client = new SmtpClient())
            {
                client.Connect(host, port, isUseSSL);
                client.Authenticate(accountName, password);
                client.Send(message);
                client.Disconnect(true);
            }
        }

        private void RetryAction(Action logic, int maxRetryCounter, Action logImportant = null, Action logError = null)
        {
            var loopCounter = 0;
            // If sending email fail then trying another 2 times
            do
            {
                loopCounter++;
                try
                {
                    logic?.Invoke();
                    break;
                }
                catch (Exception)
                {
                    logImportant?.Invoke();
                    Thread.Sleep(_appSettings.SleepPeriod);
                }
            } while (loopCounter < maxRetryCounter);
            if (loopCounter > 1)
                logError?.Invoke();
        }

        private MimeMessage CreateEmailMessage(string email, string subject, string content)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_appSettings.EmailConfig.OfficialEmailAddress));
            message.To.Add(new MailboxAddress(email));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = content };
            return message;
        }
    }
}
