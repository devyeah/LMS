using System;
using System.Collections.Generic;

namespace DevYeah.LMS.Models
{
    public partial class Resource
    {
        public Guid Id { get; set; }
        public Guid TopicId { get; set; }
        public byte Type { get; set; }
        public string Title { get; set; }
        public short DisplayOrder { get; set; }
        public string Content { get; set; }
        public byte Status { get; set; }

        public virtual Topic Topic { get; set; }
        public virtual FileRepo FileRepo { get; set; }
        public virtual PracticeRepo PracticeRepo { get; set; }
        public virtual QuizRepo QuizRepo { get; set; }
        public virtual VideoRepo VideoRepo { get; set; }
    }
}
