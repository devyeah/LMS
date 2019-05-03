using System;
using System.Collections.Generic;

namespace DevYeah.LMS.Models
{
    public partial class Account
    {
        public Account()
        {
            AccountCourse = new HashSet<AccountCourse>();
            Course = new HashSet<Course>();
            OperationHistory = new HashSet<OperationHistory>();
            TopicProgress = new HashSet<TopicProgress>();
            UserProfile = new HashSet<UserProfile>();
        }

        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public byte Status { get; set; }
        public byte Type { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ActivationTime { get; set; }
        public DateTime? LastLoginTime { get; set; }

        public virtual ICollection<AccountCourse> AccountCourse { get; set; }
        public virtual ICollection<Course> Course { get; set; }
        public virtual ICollection<OperationHistory> OperationHistory { get; set; }
        public virtual ICollection<TopicProgress> TopicProgress { get; set; }
        public virtual ICollection<UserProfile> UserProfile { get; set; }
    }
}
