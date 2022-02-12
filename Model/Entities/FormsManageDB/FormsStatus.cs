using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class FormsStatus
    {
        public FormsStatus()
        {
            FormsDBTraces = new HashSet<FormsDBTrace>();
        }

        public int StatusID { get; set; }
        public string SatusName { get; set; } = null!;

        public virtual ICollection<FormsDBTrace> FormsDBTraces { get; set; }
    }
}
