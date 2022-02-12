using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class ModelComponentSource
    {
        public ModelComponentSource()
        {
            ModelComponentMetricSourceNavigation = new HashSet<ModelComponent>();
            ModelComponentSourceNavigation = new HashSet<ModelComponent>();
        }

        public int ModelComponentSourceId { get; set; }
        public string ModelComponentSourceName { get; set; }

        public virtual ICollection<ModelComponent> ModelComponentMetricSourceNavigation { get; set; }
        public virtual ICollection<ModelComponent> ModelComponentSourceNavigation { get; set; }
    }
}
