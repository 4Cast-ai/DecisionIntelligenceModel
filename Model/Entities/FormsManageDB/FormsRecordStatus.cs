using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class FormsRecordStatus
    {
        public FormsRecordStatus()
        {
            FormsActivityTraces = new HashSet<FormsActivityTrace>();
        }

        public int RecordStatusCode { get; set; }
        public string RecordStatusName { get; set; } = null!;

        public virtual ICollection<FormsActivityTrace> FormsActivityTraces { get; set; }
    }
}
