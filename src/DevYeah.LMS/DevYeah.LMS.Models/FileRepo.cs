using System;
using System.Collections.Generic;

namespace DevYeah.LMS.Models
{
    public partial class FileRepo
    {
        public Guid Id { get; set; }
        public Guid ResourceId { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public int? Size { get; set; }

        public virtual Resource Resource { get; set; }
    }
}
