using System;

namespace DevYeah.LMS.Business.RequestModels
{
    public class UpdateUserProfileRequest
    {
        public Guid AccountId { get; set; }
        public string RecoveryEmail { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Bio { get; set; }
        //public virtual ICollection<Avatar> Avatar { get; set; }
    }
}
