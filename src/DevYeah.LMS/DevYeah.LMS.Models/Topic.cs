using System;
using System.Collections.Generic;

namespace DevYeah.LMS.Models
{
    public partial class Topic
    {
        public Topic()
        {
            Resource = new HashSet<Resource>();
            TopicProgress = new HashSet<TopicProgress>();
        }

        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string Title { get; set; }
        public short DisplayOrder { get; set; }

        public virtual Course Course { get; set; }
        public virtual ICollection<Resource> Resource { get; set; }
        public virtual ICollection<TopicProgress> TopicProgress { get; set; }
    }
}
