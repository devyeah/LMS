using System;
using System.Collections.Generic;

namespace DevYeah.LMS.Models
{
    public partial class Avatar
    {
        public Guid Id { get; set; }
        public Guid UserProfileId { get; set; }
        public string Dimension { get; set; }
        public string MimeType { get; set; }
        public short? Size { get; set; }

        public virtual UserProfile UserProfile { get; set; }
    }
}
