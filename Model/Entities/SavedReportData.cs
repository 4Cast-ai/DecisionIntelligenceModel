using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class SavedReportData
    {
        public int Id { get; set; }
        public string ReportGuid { get; set; }
        public string ReportData { get; set; }
    }
}
