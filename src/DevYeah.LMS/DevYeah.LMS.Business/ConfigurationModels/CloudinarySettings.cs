using System;
using System.Collections.Generic;
using System.Text;

namespace DevYeah.LMS.Business.ConfigurationModels
{
    public class CloudinarySettings
    {
        public string CloudName { get; set; }

        public string APIKey { get; set; }

        public string APISecret { get; set; }

        public string AvatarFolder { get; set; }
    }
}
