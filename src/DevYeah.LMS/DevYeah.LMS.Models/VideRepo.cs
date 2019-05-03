using System;
using System.Collections.Generic;

namespace DevYeah.LMS.Models
{
    public partial class VideRepo
    {
        public Guid Id { get; set; }
        public Guid ResourceId { get; set; }
        public string Url { get; set; }
        public string MimeType { get; set; }

        public virtual Resource Resource { get; set; }
    }
}
