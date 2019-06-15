using System;

namespace DevYeah.LMS.Business.ResultModels
{
    public class SignInResult
    {
        public Guid Identity { get; set; }

        public string Username { get; set; }

        public string AuthenticatedToken { get; set; }
    }
}
