using System;
using System.Collections.Generic;

namespace DevYeah.LMS.Models
{
    public partial class Resource
    {
        public Resource()
        {
            FileRepo = new HashSet<FileRepo>();
            PracticeRepo = new HashSet<PracticeRepo>();
            QuizRepo = new HashSet<QuizRepo>();
            VideRepo = new HashSet<VideRepo>();
        }

        public Guid Id { get; set; }
        public Guid TopicId { get; set; }
        public byte Type { get; set; }
        public string Title { get; set; }
        public short DisplayOrder { get; set; }
        public string Content { get; set; }
        public byte Status { get; set; }

        public virtual Topic Topic { get; set; }
        public virtual ICollection<FileRepo> FileRepo { get; set; }
        public virtual ICollection<PracticeRepo> PracticeRepo { get; set; }
        public virtual ICollection<QuizRepo> QuizRepo { get; set; }
        public virtual ICollection<VideRepo> VideRepo { get; set; }
    }
}
