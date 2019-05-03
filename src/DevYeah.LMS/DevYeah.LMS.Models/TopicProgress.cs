using System;
using System.Collections.Generic;

namespace DevYeah.LMS.Models
{
    public partial class TopicProgress
    {
        public Guid AccountId { get; set; }
        public Guid TopicId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public byte Status { get; set; }

        public virtual Account Account { get; set; }
        public virtual Topic Topic { get; set; }
    }
}
