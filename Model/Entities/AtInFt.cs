using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class AtInFt
    {
        public string ActivityTemplateGuid { get; set; }
        public string FormTemplateGuid { get; set; }

        public virtual ActivityTemplate ActivityTemplateGu { get; set; }
        public virtual FormTemplate FormTemplateGu { get; set; }
    }
}
