using System;

namespace Model.Data
{
    [Serializable]
    public class ThresholdScoreData
    {
        public double? score { get; set; }
        public int scoreLevel { get; set; }
        public CalculateNodeData OriginRef { get; set; }
    }
}
