using System;

namespace DevYeah.LMS.Business.ConfigurationModels
{
    public class TokenManagement
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
        public int Expires { get; set; }
    }
}
