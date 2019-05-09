using System;
using System.Collections.Generic;
using System.Text;

namespace DevYeah.LMS.Business.RequestModels
{
    public class ResetPasswordRequest
    {
        public Guid AccountId { get; set; }
        public string NewPassword { get; set; }
    }
}
