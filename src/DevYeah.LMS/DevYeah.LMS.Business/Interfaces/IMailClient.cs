using System.Net.Mail;

namespace DevYeah.LMS.Business.Interfaces
{
    public interface IMailClient
    {
        bool MailSent { get; }
        void Send(MailMessage message);
    }
}
