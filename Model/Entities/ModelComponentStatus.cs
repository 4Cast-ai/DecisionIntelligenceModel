using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class ModelComponentStatus
    {
        public ModelComponentStatus()
        {
            ModelComponent = new HashSet<ModelComponent>();
        }

        public int ModelComponentStatusId { get; set; }
        public string ModelComponentStatusName { get; set; }

        public virtual ICollection<ModelComponent> ModelComponent { get; set; }
    }
}
