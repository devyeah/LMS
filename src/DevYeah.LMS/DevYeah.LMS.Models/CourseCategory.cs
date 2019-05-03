using System;
using System.Collections.Generic;

namespace DevYeah.LMS.Models
{
    public partial class CourseCategory
    {
        public Guid CourseId { get; set; }
        public Guid CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Course Course { get; set; }
    }
}
