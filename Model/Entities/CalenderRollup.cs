using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class CalenderRollup
    {
        public CalenderRollup()
        {
            ModelComponent = new HashSet<ModelComponent>();
        }

        public int CalenderRollupId { get; set; }
        public string CalenderRollupName { get; set; }

        public virtual ICollection<ModelComponent> ModelComponent { get; set; }
    }
}
