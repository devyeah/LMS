using System;
using System.Collections.Generic;

namespace DevYeah.LMS.Models
{
    public partial class AccountCourse
    {
        public Guid AccountId { get; set; }
        public Guid CourseId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Account Account { get; set; }
        public virtual Course Course { get; set; }
    }
}
