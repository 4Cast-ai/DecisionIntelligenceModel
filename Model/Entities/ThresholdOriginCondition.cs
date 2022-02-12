using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class ThresholdOriginCondition
    {
        public int OriginConditionId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Threshold> ThresholdOrigin { get; set; }
    }
}
