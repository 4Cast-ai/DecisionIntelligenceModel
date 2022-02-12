using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class DynamicRecordStatus
    {
        public DynamicRecordStatus()
        {
            DynamicActivityTraces = new HashSet<DynamicActivityTrace>();
        }

        public int RecordStatusCode { get; set; }
        public string? RecordStatusName { get; set; }

        public virtual ICollection<DynamicActivityTrace> DynamicActivityTraces { get; set; }
    }
}
