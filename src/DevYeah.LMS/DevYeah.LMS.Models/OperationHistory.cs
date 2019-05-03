using System;
using System.Collections.Generic;

namespace DevYeah.LMS.Models
{
    public partial class OperationHistory
    {
        public Guid Id { get; set; }
        public byte Type { get; set; }
        public string TableName { get; set; }
        public Guid PrimaryKey { get; set; }
        public string ColumnName { get; set; }
        public string ColumnValue { get; set; }
        public DateTime? CreatedTime { get; set; }
        public Guid OperatorId { get; set; }

        public virtual Account Operator { get; set; }
    }
}
