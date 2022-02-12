using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class RollupMethod
    {
        public RollupMethod()
        {
            ModelComponent = new HashSet<ModelComponent>();
        }

        public int RollupMethodId { get; set; }
        public string RollupMethodName { get; set; }

        public virtual ICollection<ModelComponent> ModelComponent { get; set; }
    }
}
