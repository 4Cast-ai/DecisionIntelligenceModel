using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class ThresholdsDestinationCondition
    {
        public int DestinationConditionId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Threshold> ThresholdDestination { get; set; }
    }
}
