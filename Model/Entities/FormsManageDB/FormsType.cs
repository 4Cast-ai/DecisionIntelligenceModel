using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class FormsType
    {
        public FormsType()
        {
            FormsDBTraces = new HashSet<FormsDBTrace>();
        }

        public int TypeID { get; set; }
        public string TypeName { get; set; } = null!;
        public string? DBContextName { get; set; }

        public virtual ICollection<FormsDBTrace> FormsDBTraces { get; set; }
    }
}
