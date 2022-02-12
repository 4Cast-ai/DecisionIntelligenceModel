using System;

namespace Model.Data
{
    [Serializable]
    public class FormScoreData
    {
        public double FormScoreOriginal;//  { get; set; }

        public string FormScoreStringOriginal { get; set; }

        public string FormScoreComment { get; set; }
        public string MetricMeasuringUnit { get; set; }
        public string MetricGuid { get; set; }
        public string FormGuid { get; set; }
        public double FormMetricScore { get; set; }
        public bool FormScoreIsApprove { get; set; }
    }
}
