using System;
using DevYeah.LMS.Business.Interfaces;

namespace DevYeah.LMS.BusinessTest.Mock
{
    public class MailClientMocker : IEmailClient
    {
        public void SendEmail(string emailAddress, string subject, string content)
        {
            if (string.IsNullOrWhiteSpace(emailAddress) || string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(content))
                throw new ArgumentNullException();
            Console.WriteLine("Email has been sent out");
        }
    }

    public class MailClientWithExceptionMocker : IEmailClient
    {
        public void SendEmail(string email, string subject, string content)
        {
            throw new ArgumentNullException();
        }
    }
}
