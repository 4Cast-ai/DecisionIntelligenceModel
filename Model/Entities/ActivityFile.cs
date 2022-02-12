using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class ActivityFile
    {
        public string ActivityFileGuid { get; set; }
        public string ActivityGuid { get; set; }
        public string FileName { get; set; }
        public string Content { get; set; }

        public virtual Activity Activity_gu { get; set; }
    }
}
