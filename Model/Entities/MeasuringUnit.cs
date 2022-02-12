using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class MeasuringUnit
    {
        public MeasuringUnit()
        {
            ModelComponent = new HashSet<ModelComponent>();
        }

        public int MeasuringUnitId { get; set; }
        public string MeasuringUnitName { get; set; }

        public virtual ICollection<ModelComponent> ModelComponent { get; set; }
    }
}
