using System;
using System.Net.Mail;
using DevYeah.LMS.Business.Interfaces;

namespace DevYeah.LMS.Business
{
    public class MailClient : IMailClient
    {
        private readonly SmtpClient _smtpClient;
        public MailClient(SmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }
        public bool MailSent { get; private set; }

        public void Send(MailMessage message)
        {
            MailSent = false;
            if (message == null)
                throw new ArgumentNullException();

            _smtpClient.Send(message);
            MailSent = true;
        }
    }
}
