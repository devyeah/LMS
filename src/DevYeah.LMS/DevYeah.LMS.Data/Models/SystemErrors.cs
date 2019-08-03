using System;
using System.Collections.Generic;

namespace DevYeah.LMS.Data.Models
{
    public partial class SystemErrors
    {
        public Guid Id { get; set; }
        public string Exception { get; set; }
        public string CallerMemberName { get; set; }
        public int CallerLineNumber { get; set; }
        public string CallerFilePath { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
