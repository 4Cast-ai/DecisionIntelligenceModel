using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class Threshold
    {
        public string ThresholdGuid { get; set; }
        public string CreateDate { get; set; }
        public string ModifiedDate { get; set; }
        public string ModelComponentOriginGuid { get; set; }
        public int OriginCondition { get; set; }
        public double? OriginScore { get; set; }
        public int? OriginLevel { get; set; }
        public bool IsOriginLevel { get; set; }
        public string ModelComponentDestinationGuid { get; set; }
        public int DestinationCondition { get; set; }
        public double? DestinationScore { get; set; }
        public int? DestinationLevel { get; set; }
        public bool IsDestinationLevel { get; set; }
        public string AutoMessage { get; set; }
        public string FreeMessage { get; set; }
        public string Recommendations { get; set; }
        public string Explanations { get; set; }
       
        public virtual ModelComponent ModelComponentOriginGu { get; set; }
        public virtual ThresholdOriginCondition OriginConditionGu { get; set; }
        public virtual ModelComponent ModelComponentDestinationGu { get; set; }
        public virtual ThresholdsDestinationCondition DestinationConditionGu { get; set; }
    }
}
