using System;

namespace DevYeah.LMS.Models
{
    public partial class SystemErrors : IModel
    {
        public Guid Id { get; set; }
        public string Exception { get; set; }
        public string CallerMemberName { get; set; }
        public int CallerLineNumber { get; set; }
        public string CallerFilePath { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}