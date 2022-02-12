using System;

namespace Model.Data
{
    [Serializable]
    public class MetricCnvData
    {
        public string Metric_Guid { get; set; }
        public double Metric_LevelId { get; set; }
        public double? Metric_StartRange { get; set; }
        public double? Metric_EndRange { get; set; }

        //Debbie
        public double? Metric_StartRangeScoreDisplayed { get; set; }
        public double? Metric_EndRangeScoreDisplayed { get; set; }


        //Debbie
        public double? Metric_ConversionTableFinalScore { get; set; }


        public string Metric_ConversionTableScoreOrder { get; set; }

        public string DisplyLevel { get; set; }
        private bool _IsRelevante = true;
        public bool IsRelevante { get { return _IsRelevante; } set { _IsRelevante = value; } }
        public override string ToString()
        {
            return "Metric_Guid=" + Metric_Guid;
        }
    }
}
