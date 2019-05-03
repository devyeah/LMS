using System;
using System.Collections.Generic;

namespace DevYeah.LMS.Models
{
    public partial class Course
    {
        public Course()
        {
            AccountCourse = new HashSet<AccountCourse>();
            CourseCategory = new HashSet<CourseCategory>();
            Topic = new HashSet<Topic>();
        }

        public Guid Id { get; set; }
        public Guid InstructorId { get; set; }
        public string Name { get; set; }
        public string Overview { get; set; }
        public byte Edition { get; set; }
        public byte Level { get; set; }
        public double AvgLearningTime { get; set; }

        public virtual Account Instructor { get; set; }
        public virtual ICollection<AccountCourse> AccountCourse { get; set; }
        public virtual ICollection<CourseCategory> CourseCategory { get; set; }
        public virtual ICollection<Topic> Topic { get; set; }
    }
}
