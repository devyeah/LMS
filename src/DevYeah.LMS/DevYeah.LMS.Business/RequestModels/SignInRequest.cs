using System;
using System.Collections.Generic;
using System.Text;

namespace DevYeah.LMS.Business.RequestModels
{
    public class SignInRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
