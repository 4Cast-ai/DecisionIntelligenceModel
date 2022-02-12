using System;
using System.Collections.Generic;

namespace Model.Data
{
    [Serializable]
    public class ReportData
    {
        public string report_guid { get; set; }
        public string union_guid { get; set; }
        public string interface_report_guid { get; set; }
        public string model_component_guid { get; set; }
        public List<DateTime> calculated_date_list { get; set; }
        public List<UnitDataInfo> org_obj_list { get; set; }
        public List<string> activity_list { get; set; }
        public List<string> comment_list { get; set; }
        public List<string> focus_list { get; set; }
        public string report_name { get; set; }
        public string user_guid { get; set; }
        public ReportTypes report_type { get; set; }
        public bool calc_ref { get; set; }
        public int order { get; set; }
        public List<string> candidatesList { get; set; }
        public int? TemplateType { get; set; }
        public bool IsDefined { get; set; }



        public ReportData()
        {
            this.calculated_date_list = new List<DateTime>();
            this.org_obj_list = new List<UnitDataInfo>();
            this.activity_list = new List<string>();
            this.comment_list = new List<string>();
            this.focus_list = new List<string>();
        }
    }
}
