using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class EntityDescription
    {
        public string EntityGuid { get; set; }
        public int DescriptionGuid { get; set; }

        public virtual Description DescriptionGu { get; set; }
    }
}
