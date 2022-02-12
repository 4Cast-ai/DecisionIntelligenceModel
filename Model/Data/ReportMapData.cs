using System;

namespace Model.Data
{
    [Serializable]
    public class ReportMapData
    {
        public string ModelGuid { get; set; }
        public string MetricGuid { get; set; }
        public bool IsReportComments { get; set; }
        public bool IsReportFocus { get; set; }
    }
}
