using System;
using System.Collections.Generic;

namespace DevYeah.LMS.Models
{
    public partial class UserProfile
    {
        public UserProfile()
        {
            Avatar = new HashSet<Avatar>();
        }

        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string RecoveryEmail { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Bio { get; set; }

        public virtual Account Account { get; set; }
        public virtual ICollection<Avatar> Avatar { get; set; }
    }
}
