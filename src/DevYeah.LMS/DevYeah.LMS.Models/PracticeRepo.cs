using System;
using System.Collections.Generic;

namespace DevYeah.LMS.Models
{
    public partial class PracticeRepo
    {
        public Guid Id { get; set; }
        public Guid ResourceId { get; set; }
        public string Instruction { get; set; }
        public string CodeTemplate { get; set; }
        public string HelpMessage { get; set; }
        public string CorrectResult { get; set; }

        public virtual Resource Resource { get; set; }
    }
}
