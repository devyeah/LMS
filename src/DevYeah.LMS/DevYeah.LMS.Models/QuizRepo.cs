using System;
using System.Collections.Generic;

namespace DevYeah.LMS.Models
{
    public partial class QuizRepo
    {
        public Guid Id { get; set; }
        public Guid ResourceId { get; set; }
        public string Question { get; set; }
        public string Option { get; set; }
        public string Answer { get; set; }

        public virtual Resource Resource { get; set; }
    }
}
