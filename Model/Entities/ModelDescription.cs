using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class ModelDescription
    {
        public int ModelDescriptionId { get; set; }
        public string ModelComponentGuid { get; set; }
        public int? DescriptionGuid { get; set; }

        public virtual Description DescriptionGu { get; set; }
        public virtual ModelComponent ModelComponentGu { get; set; }
    }
}
