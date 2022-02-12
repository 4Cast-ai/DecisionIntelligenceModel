using System;
using System.Collections.Generic;

namespace Model.Data
{
    [Serializable]
    public class GenerateReportModelData
    {
        public string ReportGuid { get; set; }
        public string UserGuid { get; set; }
        public string ReportTypeId { get; set; }
        public int ReportType { get; set; }
        public string ReportName { get; set; }
        public string ModelGuid { get; set; }
        public string ModelName { get; set; }
        public string PolygonGuid { get; set; }
        public string PolygonName { get; set; }
        public string FixedDate { get; set; }
        public string Dates { get; set; }
        public string FormsToCalc { get; set; }
        public string ReportByActivity { get; set; }

        public string EndDate { get; set; }
        public string Save_Date { get; set; }
        public bool IsOpen10 { get; set; }
        public string IsOpen10Field { get; set; }
        public bool IsPrimary { get; set; }
        public string IsPrimaryField { get; set; }
        public bool IsSecondary { get; set; }
        public string IsSecondaryField { get; set; }
        public bool IsReportByActivites { get; set; }

        public bool IsReportMap { get; set; }
        public DateTime ReportDate { get; set; }
        public List<UnitDetails> UnitsList { get; set; }
        public List<ReportMapData> MetricsList { get; set; }
    }
}
