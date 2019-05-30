using System;
using System.Collections.Generic;
using System.Text;

namespace DevYeah.LMS.Business.RequestModels
{
    public class SignUpRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public byte Type { get; set; }
    }
}
