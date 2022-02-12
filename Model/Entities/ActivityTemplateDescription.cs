using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public partial class ActivityTemplateDescription
    {
        public int DescriptionGuid { get; set; }
        public string ActivityTemplateGuid { get; set; }

        public virtual Description DescriptionGu { get; set; }
        public virtual ActivityTemplate ActivityTemplateGu { get; set; }
    }
}
