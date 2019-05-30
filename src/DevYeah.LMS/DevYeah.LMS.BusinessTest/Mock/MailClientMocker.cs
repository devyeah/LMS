using System.Net.Mail;
using DevYeah.LMS.Business.Interfaces;

namespace DevYeah.LMS.BusinessTest.Mock
{
    public class MailClientMocker : IMailClient
    {
        public MailClientMocker()
        {
            MailSent = true;
        }
        public bool MailSent { get; set; }

        public void Send(MailMessage message)
        {
            
        }
    }
}
