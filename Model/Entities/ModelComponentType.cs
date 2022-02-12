using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class ModelComponentType
    {
        public ModelComponentType()
        {
            ModelStructure = new HashSet<ModelStructure>();
        }

        public int TypeGuid { get; set; }
        public string TypeName { get; set; }

        public virtual ICollection<ModelStructure> ModelStructure { get; set; }
    }
}
