using System.Threading.Tasks;
using DevYeah.LMS.Business.ConfigurationModels;

namespace DevYeah.LMS.Business.Interfaces
{
    public interface IEmailClient
    {
        void SendEmail(string email,  string subject, string content);
    }
}
