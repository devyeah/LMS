using System;
using System.Collections.Generic;
using System.Text;

namespace DevYeah.LMS.Business.ConfigurationModels
{
    public class EmailTemplateSettings
    {
        public string SignUpMailSubject { get; set; }
        public string PasswordRecoveryMailSubject { get; set; }
        public string SignUpMailContent { get; set; }
        public string PasswordRecoveryMailContent { get; set; }
    }
}
