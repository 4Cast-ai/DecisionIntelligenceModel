using System;
using System.Collections.Generic;

namespace Model.Entities
{
    [Serializable]
    public partial class ModelStructure
    {
        public string ModelComponentGuid { get; set; }
        public string ModelComponentParentGuid { get; set; }
        public string ModelComponentOrigionGuid { get; set; }
        public int? ModelComponentType { get; set; }
        public string AllOrigins { get; set; }
        public int Id { get; set; }

        public virtual ModelComponent ModelComponentGu { get; set; }
        public virtual ModelComponent ModelComponentOrigionGu { get; set; }
        public virtual ModelComponent ModelComponentParentGu { get; set; }
        public virtual ModelComponentType ModelComponentTypeNavigation { get; set; }
    }
}
